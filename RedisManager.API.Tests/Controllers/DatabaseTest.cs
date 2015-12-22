using System;
using System.Net;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using RedisManager.API.Controllers;
using StackExchange.Redis;

namespace RedisManager.API.Tests.Controllers
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void should_get_database_size()
        {
            var muxSubstitute = Substitute.For<IConnectionMultiplexer>();
            muxSubstitute.GetEndPoints(true).Returns(x => new EndPoint[1]);

            RedisDatabaseController controller = new RedisDatabaseController(muxSubstitute);
            var dbSize = controller.Size();

            dbSize.Should().Be(0);

        }
    }
}
