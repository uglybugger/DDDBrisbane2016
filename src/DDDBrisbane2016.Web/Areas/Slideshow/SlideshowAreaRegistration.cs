using System.Web.Mvc;

namespace DDDBrisbane2016.Web.Areas.Slideshow
{
    public class SlideshowAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Slideshow"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Slideshow_default",
                "Slideshow/{controller}/{action}/{id}",
                new {action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}