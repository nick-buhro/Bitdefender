using NickBuhro.Bitdefender.Controllers;
using Xunit;

namespace NickBuhro.Bitdefender.Tests.Controllers
{
    public abstract class ControllerTests
    {
        private readonly BitdefenderClient _client;
        private readonly Controller _controller;

        protected BitdefenderClient Client => _client;

        public ControllerTests(BitdefenderClient client, Controller controller)
        {
            _client = client;
            _controller = controller;
        }
                
        [Fact]
        public void TargetUrlTest()
        {            
            Assert.NotNull(_controller.TargetUrl);
            Assert.StartsWith(Client.AccessUrl.AbsoluteUri, _controller.TargetUrl.AbsoluteUri);
        }

        [Fact]
        public void ClassNameTest()
        {
            var controllerName = _controller.GetType().Name;
            var apiName = controllerName.Substring(0, controllerName.Length - "Controller".Length).ToLower();

            Assert.EndsWith(apiName, _controller.TargetUrl.AbsolutePath);
        }
    }
}
