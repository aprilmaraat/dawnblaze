using System;
using System.Web.Mvc;
using System.Web.Security;
using Membership.DataAccess;

namespace RedBook.Controllers
{
	public class LoginController : Controller
	{
//		private readonly UnitOfWork _membershipUnitOfWork = new UnitOfWork();
		private readonly GenericMembershipProvider _membershipProvider = new GenericMembershipProvider();

		public ActionResult Index ()
		{
			return View();
		}

		[HttpPost]
		public JsonResult Register()
		{
			var creationResult = _membershipProvider.CreateUser (new Guid(), "aj", "dawnblaze@gmail.com");

			return Json (creationResult);
		}

		[HttpPost]
		public JsonResult ValidateCredentials(string username, string password)
		{

			if (_membershipProvider.ValidateUser (username, password)) {
				
				FormsAuthentication.SetAuthCookie (username, true);

				return Json (true);

			} else {
				
				return Json (false);

			}

		}

//		[HttpPost]
//		public ActionResult RedirectToHome(){
//			
//			if (User.Identity.IsAuthenticated) {
//				
//				return RedirectToAction("Index", "Home");
//
//			} else {
//				
//				return RedirectToAction("Index", "Login");
//
//			}
//
//		}

		[HttpPost]
		public void Logout(){
			
			FormsAuthentication.SignOut();
			Session.Clear();
			Session.RemoveAll();
//			return RedirectToAction("Index", "Home");

		}

	}
}
