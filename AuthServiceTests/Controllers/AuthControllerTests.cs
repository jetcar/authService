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
            var token = ((TestCookies) controller.Response.Cookies).Get(AuthFilter.TokenHeader);
            var authorizationFilterContext = new AuthorizationFilterContext(new ActionContext(new TestHttpContext(),new RouteData(),new ActionDescriptor(),new ModelStateDictionary()),new List<IFilterMetadata>());
            ((TestCoockieCollection)authorizationFilterContext.HttpContext.Request.Cookies).Add(AuthFilter.TokenHeader,token);
            new AuthFilter(GetService<ISessionRepository>(),GetService<IUsersRepository>()).OnAuthorization(authorizationFilterContext);

            Assert.IsFalse(authorizationFilterContext.Result is ForbidResult);

        }
        [Test]
        public void TokenCheckFailTest()
        {
            var token = JwtHelper.Encode(new TokenDto());
            var authorizationFilterContext = new AuthorizationFilterContext(new ActionContext(new TestHttpContext(),new RouteData(),new ActionDescriptor(),new ModelStateDictionary()),new List<IFilterMetadata>());
            ((TestCoockieCollection)authorizationFilterContext.HttpContext.Request.Cookies).Add(AuthFilter.TokenHeader,token);
            new AuthFilter(GetService<ISessionRepository>(),GetService<IUsersRepository>()).OnAuthorization(authorizationFilterContext);

            Assert.IsTrue(authorizationFilterContext.Result is ForbidResult);

        }
    }
}