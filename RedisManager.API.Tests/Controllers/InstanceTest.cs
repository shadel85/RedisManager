using System;
using System.Collections.Generic;
using System.Net;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;
using RedisManager.API.Controllers;
using StackExchange.Redis;

namespace RedisManager.API.Tests.Controllers
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void should_get_all_keys()
        {
            var muxSubstitute = Substitute.For<IConnectionMultiplexer>();
            muxSubstitute.GetEndPoints(true).Returns(x => new EndPoint[1]);

            RedisInstanceController controller = new RedisInstanceController(muxSubstitute);

            var result = controller.Keys();

            result.Should().NotBeNull();
            result.IsFaulted.Should().BeFalse("Redis should not return a fault");
        }

        [TestMethod]
        public void should_get_server_config()
        {
           
            var muxServer = Substitute.For<IServer>();
            muxServer.ConfigGet().Returns(x => new[]
                                                {
                                                    new KeyValuePair<string, string>("lsd", "high")
                                                });
            var muxSubstitute = Substitute.For<IConnectionMultiplexer>();
            muxSubstitute.GetServer(Arg.Any<EndPoint>()).Returns(x => muxServer);
            muxSubstitute.GetEndPoints(true).Returns(x => new EndPoint[1]);
           
            RedisInstanceController controller = new RedisInstanceController(muxSubstitute);
            var config = controller.Config();

            config.Should().ContainKey("lsd");
            config.Should().ContainValue("high");
        }

    }
}
