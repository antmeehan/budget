using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.People.v1;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AntMeehan.Budget.WebApi
{
    public class AccountController : Controller
    {
        private readonly BudgetContext _budgetContext;
        private readonly IAuthorizationCodeFlow _authorizationFlow;
        private readonly IOptions<OAuthConfig> _oauthConfig;

        public AccountController(BudgetContext budgetContext, Google.Apis.Auth.OAuth2.Flows.IAuthorizationCodeFlow authorizationFlow, IOptions<OAuthConfig> oauthConfig)
        {
            _budgetContext = budgetContext;
            _authorizationFlow = authorizationFlow;
            _oauthConfig = oauthConfig;
        }

        [HttpPost]
        [Route("api/account/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var userIdNotSpecified = string.Empty;

            if (string.IsNullOrEmpty(request.GoogleCode))
            {
                return BadRequest($"Must specify {nameof(request.GoogleCode)}.");
            }

            var token = await _authorizationFlow.ExchangeCodeForTokenAsync(userIdNotSpecified, request.GoogleCode, "http://localhost:5000/logincomplete", CancellationToken.None);

            var credential = new UserCredential(_authorizationFlow, "me", token);

            var peopleService = new PeopleService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,

            });

            var me = peopleService.People.Get("people/me");

            return Ok(me.Execute());
        }

        public class LoginRequest
        {
            public string GoogleCode { get; set; }
        }
    }
}