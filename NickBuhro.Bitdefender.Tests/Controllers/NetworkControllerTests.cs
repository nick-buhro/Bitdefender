using System.Threading.Tasks;
using Xunit;

namespace NickBuhro.Bitdefender.Tests.Controllers
{
    [Collection(nameof(BitdefenderClientCollection))]
    public sealed class NetworkControllerTests : ControllerTests
    {
        public NetworkControllerTests(BitdefenderClientFixture fixture)
            : base(fixture.Client, fixture.Client.Network) { }
        
        [Fact]
        public async Task GetCompaniesListTest()
        {
            var list = await Client.Network.GetCompaniesList();
            Assert.NotNull(list);
        }
    }
}
