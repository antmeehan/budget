using System.Threading.Tasks;
using Xunit;
using AntMeehan.Budget.WebApi;
using System.Net;
using Shouldly;

namespace AntMeehan.Budget.Tests
{

    public class AuthenticationTests : TestBase
    {

        [Fact]
        public async Task GivenUserNotInStore_WhenLogin_ThenUserCreatedAndBearerTokeReturned()
        {
            // When
            var response = await PostAsync("api/account/login", new AccountController.LoginRequest()
            {
                GoogleAccessToken = "TOKEN123"
            });

            // Then
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

        }
    }
}