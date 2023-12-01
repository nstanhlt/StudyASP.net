using System.Threading.Tasks;
using aow3.Models.TokenAuth;
using aow3.Web.Controllers;
using Shouldly;
using Xunit;

namespace aow3.Web.Tests.Controllers
{
    public class HomeController_Tests: aow3WebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}