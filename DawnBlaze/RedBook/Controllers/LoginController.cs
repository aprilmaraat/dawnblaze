using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Mvc.Ajax;
using Membership.Data;
using Membership.DataAccess;
using RedBook.Models;

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

		public JsonResult Register()
		{
			var creationResult = _membershipProvider.CreateUser (new Guid(), "aj", "dawnblaze@gmail.com");

			return Json (creationResult, JsonRequestBehavior.AllowGet);
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			Session.Clear();
			Session.RemoveAll();
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public JsonResult ValidateCredentials(Login login)
		{
			var validationResult = _membershipProvider.ValidateUser(login.Username, login.Password);

			if(validationResult){
				FormsAuthentication.SetAuthCookie(login.Username, false);
			}

			return Json (validationResult);
		}

	}
}
