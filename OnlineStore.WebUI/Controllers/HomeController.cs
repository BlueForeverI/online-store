using System.Web.Mvc;

namespace OnlineStore.WebUI.Controllers
{
    //[OutputCache(CacheProfile = "StaticUser")]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            
            return View();
        }
    }
}