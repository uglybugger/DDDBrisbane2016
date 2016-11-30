using System.Web.Mvc;

namespace DDDBrisbane2016.Web.Controllers
{
    public class SlideshowController : Controller
    {
        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult Start()
        {
            return Redirect("/slideshow/display/howtoplay");
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult Display(string id)
        {
            var viewName = id;
            return View(viewName);
        }
    }
}