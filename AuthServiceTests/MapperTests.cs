using System;
using System.Collections.Generic;
using System.Text;
using AuthService;
using AutoMapper;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AuthServiceTests
{
    [TestFixture]
    public class MapperTests
    {
        [Test]
        public void TestConfiguration()
        {
            var config = new MapperConfiguration(new MapperProfile());
            config.AssertConfigurationIsValid();
        }
    }
}