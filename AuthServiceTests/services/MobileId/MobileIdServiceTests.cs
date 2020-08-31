using AuthService;
using AutoMapper;
using MobileId.MobileId;
using NUnit.Framework;

namespace AuthServiceTests.services.MobileId
{
    [TestFixture]
    public class MobileIdServiceTests
    {
        [Test]
        public void CodeTest()
        {
            var code = MobileIdService.GetCodeFromHash("0nbgC2fVdLVQFZJdBbmG7oPoElpCYsQMtrY0c0wKYRg=");
            Assert.AreEqual("6680", code);
        }
    }
}