using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RedisManager.API.Controllers;

namespace RedisManager.API.Tests.Controllers
{
    [TestClass]
    public class ConnectTest
    {
        [TestMethod]
        public async Task should_connect_to_instance()
        {
            //TODO: Is this test / functionality required!?
            const string redisServer = "localhost";
            const int redisPort = 6000;

            RedisConnectController controller = new RedisConnectController();

            var redisConnection = await controller.Connect(redisServer, redisPort);

            Assert.IsTrue(redisConnection.IsConnected);
        }
    }
}
