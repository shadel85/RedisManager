using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using RedisManager.API.Controllers;
using StackExchange.Redis;

namespace RedisManager.API.Tests.Controllers
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public async Task should_get_clients()
        {
            var muxSubstitute = Substitute.For<IConnectionMultiplexer>();
            muxSubstitute.GetEndPoints(true).Returns(x => new EndPoint[1]);

            RedisClientController controller = new RedisClientController(muxSubstitute);

            var clients = await controller.Clients();

            clients.Count.Should().Be(0);

        }
    }
}
