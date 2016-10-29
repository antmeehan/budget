using System.Threading.Tasks;
using Xunit;
using AntMeehan.Budget.WebApi;
using System.Net;
using Shouldly;
using Google.Apis.Auth.OAuth2.Flows;

namespace AntMeehan.Budget.Tests
{

    public class AuthenticationTests : TestBase
    {

        [Fact]
        public async Task GivenUserNotInStore_WhenLogin_ThenUserCreatedAndBearerTokeReturned()
        {
            SubsituteAndConfigure<IAuthorizationCodeFlow>();
            // When
            var response = await PostAsync("api/account/login", new AccountController.LoginRequest()
            {
                GoogleCode = "CODE123"
            });

            // Then
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

        }
    }
}