using System.Web.Mvc;
using DDDBrisbane2016.Web.Areas.Slideshow.Controllers;
using MvcNavigationHelpers;

namespace DDDBrisbane2016.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly UrlHelper _urlHelper;

        public HomeController(UrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public RedirectResult Index()
        {
            var url = _urlHelper.UrlFor<PresentController>(c => c.Start());
            return Redirect(url);
        }
    }
}