using System;
using System.Collections.Generic;
using AuthService.Controllers;
using AuthService.Security;
using Dto;
using NUnit.Framework;

namespace AuthServiceTests.Controllers
{
    public class UserProfileControllerTests : TestsBase
    {
        [Test]
        public void GetUserProfile()
        {
            CreateNewUser(out var username, out var password);
            var session = GetUserSession(username, password);

            var controller = GetController<UserProfileController>(session);
            //act
            var profiledto = controller.UserProfile();
            //assert

            Assert.AreEqual(username, profiledto.Username);
        }

        [Test]
        public void UpdateUserProfile()
        {
            CreateNewUser(out var username, out var password);
            var session = GetUserSession(username, password);

            var controller = GetController<UserProfileController>(session);
            var profiledto = controller.UserProfile();

            //act
            GetController<UserProfileController>(session).UpdateUserProfile(new EditUserDto() { NewPassword = "1234" });
            //assert

            Assert.Throws<UnauthorizedAccessException>(() => GetUserSession(username, password));
            var session2 = GetUserSession(username, "1234");
            Assert.IsFalse(string.IsNullOrEmpty(session2));
        }
    }
}