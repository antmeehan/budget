using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.People.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;

namespace AntMeehan.Budget.WebApi
{
    public class AccountController : Controller
    {
        private readonly BudgetContext _budgetContext;
        private readonly IAuthorizationCodeFlow _authorizationFlow;

        public AccountController(BudgetContext budgetContext, Google.Apis.Auth.OAuth2.Flows.IAuthorizationCodeFlow authorizationFlow)
        {
            _budgetContext = budgetContext;
        }

        [HttpPost]
        [Route("api/account/login")]
        public IActionResult Login(LoginRequest request)
        {
            var token = new TokenResponse()
            {
                AccessToken = request.GoogleAccessToken
            };

            var credential = new UserCredential(_authorizationFlow, "me", token);

            var peopleService = new PeopleService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            var x = _budgetContext.Budgets.Count();
            return BadRequest();
        }

        public class LoginRequest
        {
            public string GoogleAccessToken { get; set; }
        }
    }
}