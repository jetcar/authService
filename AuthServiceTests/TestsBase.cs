using System;
using System.Collections.Generic;
using AuthService;
using AuthService.Controllers;
using AuthService.Security;
using AuthServiceTests.Controllers;
using CommonTools.Utils;
using Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Repository.DbModel;

namespace AuthServiceTests
{
    public class TestsBase
    {
        private ServiceProvider _serviceProvider;

        private bool _firstInit;
        private List<IServiceScope> _currentScope = new List<IServiceScope>();

        [SetUp]
        public void Setup()
        {
            if (!_firstInit)
            {
                Environment.SetEnvironmentVariable("db_password", "q");
                Environment.SetEnvironmentVariable("db_username", "admin");
                Environment.SetEnvironmentVariable("db_name", "my_test_db");
                Environment.SetEnvironmentVariable("secret", "test123");

                MyDbContext db = new MyDbContext(null);
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                _firstInit = true;
                Startup authService = new Startup(new TestConfiguration());
                IServiceCollection services = new ServiceCollection();

                authService.ConfigureServices(services);
                _serviceProvider = services.BuildServiceProvider();
            }

            _currentScope.Add(_serviceProvider.CreateScope());
        }

        [TearDown]
        public void TearDown()
        {
            foreach (var serviceScope in _currentScope)
            {
                serviceScope.Dispose();
            }
            _currentScope.Clear();
        }

        public T GetService<T>()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetService<T>();
        }

        public T GetScopedService<T>()
        {
            return _currentScope[0].ServiceProvider.GetService<T>();
        }

        public T GetController<T>(string token = null) where T : ControllerBase
        {
            var controller = GetScopedService<T>();
            controller.ControllerContext.HttpContext = new TestHttpContext();
            if (token != null)
            {
                var session = GetScopedService<SessionDto>();
                var sessionDto = JwtHelper.Decode<SessionDto>(token);

                session.Username = sessionDto.Username;
                session.ExpirationTime = sessionDto.ExpirationTime;
                session.Password = sessionDto.Password;
                session.SessionId = sessionDto.SessionId;
                session.UserId = sessionDto.UserId;
            }

            return controller;
        }

        public string GetUserSession(string username, string password)
        {
            var controller = GetController<AuthController>();
            controller.Login(new LoginDto() { Username = username, Password = password });
            var token = ((TestCookies)controller.Response.Cookies).Get(AuthFilter.TokenHeader);
            return token;
        }

        public void CreateNewUser(out string username, out string password)
        {
            var controller = GetController<SignUpController>();

            var dto = new RegisterDto()
            {
                Username = MyRandom.NextString(10),
                Password = MyRandom.NextString(8)
            };
            username = dto.Username;
            password = dto.Password;
            var responseDto = controller.Register(dto);
        }
    }
}