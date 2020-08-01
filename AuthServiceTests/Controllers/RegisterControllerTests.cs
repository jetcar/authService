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
    public class RegisterControllerTests : TestsBase
    {


        [Test]
        public void RegisterThenLoginTest()
        {
            var controller = GetController<SignUpController>();

            var dto = new RegisterDto()
            {
                Username = MyRandom.NextString(10),
                Password = MyRandom.NextString(8)
            };

            //act
            var responseDto =controller.Register(dto);
            //assert
            var authController = GetController<AuthController>();
            var userDto = authController.Login(new LoginDto() { Username = dto.Username, Password = dto.Password });

            Assert.AreEqual(dto.Username, userDto.Username);
            var token = ((TestCookies)authController.Response.Cookies).Get(AuthFilter.TokenHeader);
            var authorizationFilterContext = new AuthorizationFilterContext(new ActionContext(new TestHttpContext(), new RouteData(), new ActionDescriptor(), new ModelStateDictionary()), new List<IFilterMetadata>());
            ((TestCoockieCollection)authorizationFilterContext.HttpContext.Request.Cookies).Add(AuthFilter.TokenHeader, token);
            new AuthFilter(GetService<ISessionRepository>(), GetService<IUsersRepository>()).OnAuthorization(authorizationFilterContext);

            Assert.IsFalse(authorizationFilterContext.Result is ForbidResult);

        }

      
    }
   
}