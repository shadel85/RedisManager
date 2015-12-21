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
    public class RedisClientController : ApiController
    {
        IConnectionMultiplexer _RedisMUX;

        public RedisClientController(IConnectionMultiplexer connectionMultiplexer)
        {
            _RedisMUX = connectionMultiplexer;
        }

        public async Task<List<ClientInfo>> Clients()
        {
            var clients = new List<ClientInfo>();

            var endpoints = _RedisMUX.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _RedisMUX.GetServer(endpoint);
                var clientInfo = await server.ClientListAsync();
                clients.AddRange(clientInfo.ToList());
            }
            return clients;
        }
    }
}
