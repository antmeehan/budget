using Autofac;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;

namespace AntMeehan.Budget.WebApi
{
    public class AutofacModule : Module
    {
        override protected void Load(ContainerBuilder builder)
        {

            builder.RegisterType<BudgetContext>().AsSelf();
            builder.Register(context => new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer()
            {
                ClientSecrets = new ClientSecrets()
                {
                    ClientId = Microsoft.Extensions.Configuration.["GoogleClientId"],
                    ClientSecret = Config.OAuth.GoogleClientSecret,
                }
            })).As<GoogleAuthorizationCodeFlow>();
        }
    }
}