using Autofac;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.Extensions.Options;

namespace AntMeehan.Budget.WebApi
{
    public class AutofacModule : Module
    {
        override protected void Load(ContainerBuilder builder)
        {

            builder.RegisterType<BudgetContext>().AsSelf();
            builder.Register(context =>
            {
                var oauthConfig = context.Resolve<IOptions<OAuthConfig>>().Value;

                return new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
                {

                    ClientSecrets = new ClientSecrets()
                    {
                        ClientId = oauthConfig.GoogleClientId,
                        ClientSecret = oauthConfig.GoogleClientSecret,
                    }
                });
            }).As<IAuthorizationCodeFlow>();
        }
    }
}