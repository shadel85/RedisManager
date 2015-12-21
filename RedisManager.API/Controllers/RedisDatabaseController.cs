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
    public class RedisDatabaseController : ApiController
    {
        IConnectionMultiplexer _RedisMUX;
        public RedisDatabaseController(IConnectionMultiplexer multiplexConnection)
        {
            _RedisMUX = multiplexConnection;
        }

        public long Size()
        {
            long size = 0;
            
            var endpoints = _RedisMUX.GetEndPoints(true);
            foreach (var endpoint in endpoints)
            {
                var server = _RedisMUX.GetServer(endpoint);
                if (server != null)
                    size = size + server.DatabaseSize();
            }

            return size;
        }
    }
}
