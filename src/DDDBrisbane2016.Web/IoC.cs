using Autofac;
using Autofac.Builder;

namespace DDDBrisbane2016.Web
{
    public static class IoC
    {
        public static IContainer LetThereBeIoC(ContainerBuildOptions containerBuildOptions = ContainerBuildOptions.None)
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(IoC).Assembly);
            return builder.Build(containerBuildOptions);
        }
    }
}