using System;
using System.Collections.Generic;
using AuthService.Controllers;
using AuthService.DbModel;
using AuthService.Dto;
using AuthService.Security;
using AuthService.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;


namespace AuthServiceTests.Controllers
{
    public class AuthControllerTests : TestsBase
    {


        [Test]
        public void LoginTest()
        {
            var controller = GetController<AuthController>();
            var userDb = new UserDb()
            {
                Username = "test1",
                PasswordSalt = Guid.NewGuid().ToString(),
            };
            userDb.SetPassword("test1234");
            var dbContext = GetService<MyDbContext>();

            dbContext.Users.Add(userDb);
            dbContext.SaveChanges();

            //act
            var userDto = controller.Login(new LoginDto() { Username = userDb.Username, Password = "test1234" });
            //assert
            Assert.AreEqual(userDb.Username, userDto.Username);
            var token = ((TestCookies)controller.Response.Cookies).Get(AuthFilter.TokenHeader);
            var authorizationFilterContext = new AuthorizationFilterContext(new ActionContext(new TestHttpContext(), new RouteData(), new ActionDescriptor(), new ModelStateDictionary()), new List<IFilterMetadata>());
            ((TestCoockieCollection)authorizationFilterContext.HttpContext.Request.Cookies).Add(AuthFilter.TokenHeader, token);
            new AuthFilter(GetService<ISessionRepository>(), GetService<IUsersRepository>()).OnAuthorization(authorizationFilterContext);

            Assert.IsFalse(authorizationFilterContext.Result is ForbidResult);

        }

        [Test]
        public void LogoutTest()
        {
            var controller = GetController<AuthController>();
            var userDb = new UserDb()
            {
                Username = MyRandom.NextString(15),
                PasswordSalt = Guid.NewGuid().ToString(),
            };
            userDb.SetPassword("test1234");
            var dbContext = GetService<MyDbContext>();

            dbContext.Users.Add(userDb);
            dbContext.SaveChanges();
            var userDto = controller.Login(new LoginDto() { Username = userDb.Username, Password = "test1234" });
            var token = ((TestCookies)controller.Response.Cookies).Get(AuthFilter.TokenHeader);
            controller = GetController<AuthController>();
            controller.HttpContext.Items[AuthFilter.SessionData] = JwtHelper.Decode<TokenDto>(token);
            //act
            controller.Logout();
            //assert
            Assert.AreEqual(userDb.Username, userDto.Username);

            var authorizationFilterContext = new AuthorizationFilterContext(new ActionContext(new TestHttpContext(), new RouteData(), new ActionDescriptor(), new ModelStateDictionary()), new List<IFilterMetadata>());
            ((TestCoockieCollection)authorizationFilterContext.HttpContext.Request.Cookies).Add(AuthFilter.TokenHeader, token);
            new AuthFilter(GetService<ISessionRepository>(), GetService<IUsersRepository>()).OnAuthorization(authorizationFilterContext);

            Assert.IsTrue(authorizationFilterContext.Result is ForbidResult);
        }
        [Test]
        public void TokenCheckFailTest()
        {
            var token = JwtHelper.Encode(new TokenDto());
            var authorizationFilterContext = new AuthorizationFilterContext(new ActionContext(new TestHttpContext(), new RouteData(), new ActionDescriptor(), new ModelStateDictionary()), new List<IFilterMetadata>());
            ((TestCoockieCollection)authorizationFilterContext.HttpContext.Request.Cookies).Add(AuthFilter.TokenHeader, token);
            new AuthFilter(GetService<ISessionRepository>(), GetService<IUsersRepository>()).OnAuthorization(authorizationFilterContext);

            Assert.IsTrue(authorizationFilterContext.Result is ForbidResult);

        }
    }
    public class MyRandom
    {
        private static Random random = new Random();
        private static char[] _chars = new char[]{'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','r','s','t','u','v','w','q','x','y','z'};
        public static string NextString(int length)
        {
            var list = new List<char>();
            for (int i = 0; i < length; i++)
            {
                list.Add(_chars[random.Next(0,_chars.Length-1)]);
            }
            return  new string(list.ToArray());
        }
    }
}