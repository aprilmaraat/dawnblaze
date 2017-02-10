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
		private readonly GenericMembershipProvider _membershipProvider = new GenericMembershipProvider();

		[AllowAnonymous]
		public ActionResult Index ()
		{
			if (HttpContext.User.Identity.IsAuthenticated) {
				return RedirectToAction ("Logout", "Login");
			} 
			else {
				return View();
			}
		}

		[HttpPost]
		public JsonResult ValidateCredentials(Login login)
		{
			var validationResult = _membershipProvider.ValidateUser (login.Username, login.Password);

			if (validationResult) {
				FormsAuthentication.SetAuthCookie (login.Username, false);
			}

			return Json (validationResult);
		}

		[AllowAnonymous]
		public ActionResult Register(){
			return View ();
		}

		[HttpPost]
		public JsonResult RegisterJson(Register register)
		{
			var registerModelState = new RegisterModelState ();

			if (!ModelState.IsValid) {
				registerModelState = GetErrorsFromModelState ();

				if (register.ConfirmPassword != register.Password) {
					registerModelState.ConfirmPassword = "Confirm Password did not match.";
				}

				return Json (new { isSuccess = false, error = registerModelState });
			} 
			else {
				if (register.ConfirmPassword != register.Password) {
					
//					var registerModelState = new RegisterModelState
//					{ 
//						Username = "",
//						Password = "",
//						Email = "",
//						ConfirmPassword = "Confirm Password did not match."
//					};

					registerModelState.Username = "";
					registerModelState.Password = "";
					registerModelState.Email = "";
					registerModelState.ConfirmPassword = "Confirm Password did not match.";

					return Json (new { isSuccess = false, error = registerModelState });
				} 
				else {
					var creationResult = _membershipProvider.CreateUser (register.Username, register.Password, register.Email);

					return Json (new { isSuccess = creationResult });
				}
			}

			return Json (GetErrorsFromModelState());

		}

		[AllowAnonymous]
		public ActionResult ForgotPassword(){
			return View ();
		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			Session.Clear();
			Session.RemoveAll();
			return RedirectToAction("Index", "Login");
		}

		private RegisterModelState GetErrorsFromModelState()
		{
//			return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
			var registerModelState = new RegisterModelState();

			foreach(var key in ModelState.Keys){

				var value = ModelState[key];

				var errorMessage = "";

				foreach (var error in value.Errors) {
					errorMessage += error.ErrorMessage;
				}

				if(key == "Username"){
					registerModelState.Username = errorMessage;
				}
				else if(key == "Password"){
					registerModelState.Password = errorMessage;
				}
				else if(key == "Email"){
					registerModelState.Email = errorMessage;
				}
				else if(key == "ConfirmPassword"){
					registerModelState.ConfirmPassword = errorMessage;
				}

			}

			return registerModelState;

		}

	}
}
