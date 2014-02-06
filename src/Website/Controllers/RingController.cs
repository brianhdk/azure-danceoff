using System.Web.Mvc;

namespace Website.Controllers
{
	[Authorize(Users = Models.User.Brianh)]
    public class RingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}