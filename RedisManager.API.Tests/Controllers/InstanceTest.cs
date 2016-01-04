﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Core;
using RedisManager.API.Controllers;
using StackExchange.Redis;

namespace RedisManager.API.Tests.Controllers
{
    [TestClass]
    public class InstanceTest
    {
        [TestMethod]
        public async Task should_get_all_keys()
        {           
            var muxServer = Substitute.For<IServer>();
            muxServer.Keys().Returns(x => new List<RedisKey>()
                                                {
                                                    "lsd"
                                                });
            var muxSubstitute = Substitute.For<IConnectionMultiplexer>();
            muxSubstitute.GetServer(Arg.Any<EndPoint>()).Returns(x => muxServer);
            muxSubstitute.GetEndPoints(Arg.Any<bool>()).Returns(x => new EndPoint[1]);

            RedisInstanceController controller = new RedisInstanceController(muxSubstitute);
            var result = await controller.Keys();

            result.Should().Contain("lsd");
        }

        [TestMethod]
        public async Task should_get_value_for_key()
        {
            var muxSubstitute = Substitute.For<IConnectionMultiplexer>();
            var databaseSubstitute = Substitute.For<IDatabase>();
            muxSubstitute.GetDatabase().Returns(databaseSubstitute);
            databaseSubstitute.StringGetAsync("lsd").Returns("high");

            RedisInstanceController controller = new RedisInstanceController(muxSubstitute);
            var value = await controller.Value("lsd");

            value.ToLower().Should().Be("high");  
        }

        [TestMethod]
        public async Task should_get_server_config()
        {
           
            var muxServer = Substitute.For<IServer>();
            muxServer.ConfigGet().Returns(x => new[]
                                                {
                                                    new KeyValuePair<string, string>("lsd", "high")
                                                });
            var muxSubstitute = Substitute.For<IConnectionMultiplexer>();
            muxSubstitute.GetServer(Arg.Any<EndPoint>()).Returns(x => muxServer);
            muxSubstitute.GetEndPoints(Arg.Any<bool>()).Returns(x => new EndPoint[1]);
           
            RedisInstanceController controller = new RedisInstanceController(muxSubstitute);
            var config = await controller.Config();

            config.Should().ContainKey("lsd");
            config.Should().ContainValue("high");
        }

        [TestMethod]
        public async Task should_get_info()
        {
            var dummyValuesDict = new Dictionary<string, string>{ { "lsd", "high"} };
            var lstKeys = dummyValuesDict.ToArray();
            var dummyGrouping = lstKeys.GroupBy(x => x.Key).ToArray();
            var muxSubstitute = Substitute.For<IConnectionMultiplexer>();
            var muxServer = Substitute.For<IServer>();
            muxSubstitute.GetServer(Arg.Any<EndPoint>()).Returns(muxServer);
            muxSubstitute.GetEndPoints(Arg.Any<bool>()).Returns(x => new EndPoint[1]);
            muxServer.InfoAsync().Returns(dummyGrouping);

            RedisInstanceController controller = new RedisInstanceController(muxSubstitute);
            var info = await controller.Info();

            info.Should().ContainSingle(x => x.ContainsKey("lsd"));
        }

        [TestMethod]
        public void should_get_status()
        {
            var muxSubstitute = Substitute.For<IConnectionMultiplexer>();
            muxSubstitute.GetStatus().Returns("TestStatus");

            RedisInstanceController controller = new RedisInstanceController(muxSubstitute);
            var status = controller.Status();

            status.Should().Be("TestStatus");
        }
    }
}
