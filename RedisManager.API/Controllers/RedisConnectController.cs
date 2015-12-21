using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using StackExchange.Redis;

namespace RedisManager.API.Controllers
{
    public class RedisConnectController : ApiController
    {
        StackExchange.Redis.ConnectionMultiplexer _RedisMux;

        public async Task<ConnectionMultiplexer> Connect(string serverName, int port)
        {
            return _RedisMux ?? (_RedisMux = await ConnectionMultiplexer.ConnectAsync($"{serverName}:{port}"));
        }
    }
}
