using Autofac;
using Autofac.Integration.Mvc;

namespace DDDBrisbane2016.Web.AutofacModules
{
    public class MvcModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterControllers(ThisAssembly);
            builder.RegisterFilterProvider();
        }
    }
}