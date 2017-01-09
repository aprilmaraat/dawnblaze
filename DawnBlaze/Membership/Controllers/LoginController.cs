using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Membership.Data;
using Membership.DataAccess;

namespace RedBook.Controllers
{
	public class LoginController : Controller
	{
		private readonly UnitOfWork _membershipUnitOfWork = new UnitOfWork();
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

		public JsonResult ValidateCredentials(string username, string password)
		{
			var validationResult = _membershipProvider.ValidateUser(username, password);

			return Json (validationResult, JsonRequestBehavior.AllowGet);
		}

	}
}
