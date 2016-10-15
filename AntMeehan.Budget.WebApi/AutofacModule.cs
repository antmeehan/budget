using Autofac;

namespace AntMeehan.Budget.WebApi {
    public class AutofacModule : Module {
        override protected void Load(ContainerBuilder builder){

            builder.RegisterType<BudgetContext>().AsSelf();
        }
    }
}