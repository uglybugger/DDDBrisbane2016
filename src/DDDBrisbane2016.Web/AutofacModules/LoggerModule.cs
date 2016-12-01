using Autofac;
using AutofacSerilogIntegration;

namespace DDDBrisbane2016.Web.AutofacModules
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterLogger();
        }
    }
}