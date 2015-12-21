using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace RedisManager.API.Controllers
{
    public class RedisInstanceController : ApiController
    {
        IConnectionMultiplexer _RedisMUX;
        public RedisInstanceController(IConnectionMultiplexer multiplexConnection)
        {
            _RedisMUX = multiplexConnection;
        }

        public async Task<List<RedisKey>> Keys()
        {
            var endpoints = _RedisMUX.GetEndPoints(true);
            List<RedisKey> keys = new List<RedisKey>();
            foreach (var endpoint in endpoints)
            {
                var server = _RedisMUX.GetServer(endpoint);
                keys.AddRange(server.Keys());
            }
            return keys;
        }

        public Dictionary<string, string> Config()
        {
            Dictionary<string, string> configs = new Dictionary<string, string>();
            var endpoints = _RedisMUX.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _RedisMUX.GetServer(endpoint);
                foreach (var config in server.ConfigGet())
                {
                    configs.Add(config.Key, config.Value);
                }
            }

            return configs;
        }
    }
}
