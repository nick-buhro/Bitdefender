using System;
using System.Threading.Tasks;
using Xunit;
using NickBuhro.Bitdefender.Models;
using NickBuhro.Bitdefender.Controllers;

namespace NickBuhro.Bitdefender.Tests.Controllers
{
    [Collection(nameof(BitdefenderClientCollection))]
    public sealed class CompaniesControllerTests: ControllerTests
    {
        public const string TestCompanySuffix = " [ATest]";

        public CompaniesControllerTests(BitdefenderClientFixture fixture)
            : base(fixture.Client, fixture.Client.Companies) { }
        

        /// <summary>
        /// Complex test that reflects all company lifecycle: creation, modifying, getting info and deleting.
        /// </summary>
        [Fact]
        public async Task CompositeTest()
        {
            var type = CompanyType.Customer;
            var name = Guid.NewGuid().ToString("N").Substring(0, 8) + TestCompanySuffix;            

            // Create company

            var id = await Client.Companies.CreateCompany(type, name);

            Assert.NotNull(id);
            Assert.NotEqual("", id);

            // Check details

            var details = await Client.Companies.GetCompanyDetails(id);

            Assert.Equal(type, details.Type);
            Assert.Equal(name, details.Name);

            // Delete company

            await Client.Companies.DeleteCompany(id);

            // Check if deleted

            await Assert.ThrowsAsync<JsonRpcException>(async () =>
            {
                await Client.Companies.GetCompanyDetails(id);
            });
        }

        /// <summary>
        /// Maintenance code that deletes test companies from the company tenant.
        /// </summary>
        [Fact]
        public async Task Cleanup()
        {
            var cmpList = await Client.Network.GetCompaniesList();
            foreach (var c in cmpList)
            {
                if (c.Name.EndsWith(TestCompanySuffix))
                    await Client.Companies.DeleteCompany(c.Id);
            }
        }        
    }
}
