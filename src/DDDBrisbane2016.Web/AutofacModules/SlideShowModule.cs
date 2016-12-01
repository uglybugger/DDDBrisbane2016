using Autofac;
using DDDBrisbane2016.Web.SlideShow;

namespace DDDBrisbane2016.Web.AutofacModules
{
    public class SlideShowModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AndrewsSlideSequence>()
                .AsSelf()
                .SingleInstance();
        }
    }
}