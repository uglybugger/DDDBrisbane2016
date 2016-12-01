using System.Linq;
using System.Web.Mvc;
using DDDBrisbane2016.Web.SlideShow;

namespace DDDBrisbane2016.Web.Controllers
{
    public class SlideshowController : Controller
    {
        private readonly AndrewsSlideSequence _slideSequence;

        public SlideshowController(AndrewsSlideSequence slideSequence)
        {
            _slideSequence = slideSequence;
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public ActionResult Display(string id)
        {
            var viewName = id;
            return View(viewName);
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public RedirectResult Start()
        {
            var slideName = _slideSequence.GetFirst();
            return RedirectToSlide(slideName);
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public RedirectResult Previous()
        {
            if (Request.UrlReferrer == null) return Start();

            var tokens = Request.UrlReferrer.AbsolutePath.Split("/".ToCharArray());
            var currentSlide = tokens.Last();
            if (string.IsNullOrWhiteSpace(currentSlide)) return Start();

            string previousSlide;
            if (_slideSequence.TryGetPrevious(currentSlide, out previousSlide))
            {
                return RedirectToSlide(previousSlide);
            }

            return Start();
        }

        [HttpGet]
        [OutputCache(Duration = 0)]
        public RedirectResult Next()
        {
            if (Request.UrlReferrer == null) return Start();

            var tokens = Request.UrlReferrer.AbsolutePath.Split("/".ToCharArray());
            var currentSlide = tokens.Last();
            if (string.IsNullOrWhiteSpace(currentSlide)) return Start();

            string nextSlide;
            if (_slideSequence.TryGetNext(currentSlide, out nextSlide))
            {
                return RedirectToSlide(nextSlide);
            }

            return RedirectToSlide(currentSlide);
        }

        private RedirectResult RedirectToSlide(string slideName)
        {
            return Redirect($"/slideshow/display/{slideName}");
        }
    }
}