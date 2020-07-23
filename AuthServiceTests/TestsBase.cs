using System;
using System.Collections.Generic;
using AuthService;
using AuthService.DbModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

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
                Environment.SetEnvironmentVariable("db_password","q");
                Environment.SetEnvironmentVariable("db_username","admin");
                Environment.SetEnvironmentVariable("db_name","my_test_db");
                Environment.SetEnvironmentVariable("secret","test123");

                MyDbContext db = new MyDbContext();
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
        public T GetController<T>() where T : ControllerBase
        {
            var controller = GetScopedService<T>();
            controller.ControllerContext.HttpContext = new TestHttpContext();
            return controller;
        }
    }
}