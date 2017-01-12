using System.Web.Mvc;

namespace RedBook.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index ()
		{
			if (User.Identity.IsAuthenticated) {
				
				return View ();

			} else {
				
				return RedirectToAction("Index", "Login");

			}
		}
	}
}