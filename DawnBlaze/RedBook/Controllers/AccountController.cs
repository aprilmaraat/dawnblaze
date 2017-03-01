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
	public class AccountController : Controller
	{
		private readonly UnitOfWork _unitOfWork = new UnitOfWork();
		private readonly GenericMembershipProvider _membershipProvider = new GenericMembershipProvider();

		//[AllowAnonymous]
		//public JsonResult Test() { 
			
		//}

		[AllowAnonymous]
		public ActionResult Index ()
		{
			if (HttpContext.User.Identity.IsAuthenticated) {
				return RedirectToAction ("Logout", "Account");
			}
			else {
				return View();
			}
		}

		[HttpPost]
		public JsonResult Login(Login login)
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
				registerModelState = GetErrorsFromRegisterModelState ();

				if (register.ConfirmPassword != register.Password) {
					registerModelState.ConfirmPassword = "Confirm password did not match.";
				}

				return Json (new { isSuccess = false, error = registerModelState });
			} 
			else {

				registerModelState.Username = "";
				registerModelState.Password = "";
				registerModelState.Email = "";
				registerModelState.ConfirmPassword = "";

				var userExist = _unitOfWork.UserRepository.GetOne (q => q.UserName == register.Username) != null;
				var emailExist = _unitOfWork.UserRepository.GetOne (q => q.Email == register.Email) != null;
				var passwordMinimumLength = register.Password.Length >= 8;

				if (userExist || emailExist || passwordMinimumLength == false || register.ConfirmPassword != register.Password) {

					if(userExist){
						registerModelState.Username = "Username already exist.";
					}

					if(emailExist){
						registerModelState.Email = "Email already exist.";
					}

					if(passwordMinimumLength == false){
						registerModelState.Password = "Password must be at least 8 characters long.";
					}

					if (register.ConfirmPassword != register.Password) {
						registerModelState.ConfirmPassword = "Confirm Password did not match.";
					}

					return Json (new { isSuccess = false, error = registerModelState });
				} 
				else {

					var creationResult = _membershipProvider.CreateUser (register.Username, register.Password, register.Email);

					return Json (new { isSuccess = creationResult });

				}

			}

		}

		[AllowAnonymous]
		public ActionResult ForgotPassword(){
			return View ();
		}

		public JsonResult ForgotPasswordJson(ForgotPassword forgotPassword){

			var forgotPasswordModelState = new ForgotPasswordModelState ();

			if (!ModelState.IsValid) {
				forgotPasswordModelState = GetErrorsFromForgotPasswordModelState ();

				return Json (new { isSuccess = false, error =  forgotPasswordModelState });
			} 
			else {

				if (_unitOfWork.UserRepository.GetOne (q => q.Email == forgotPassword.Email) != null) {
					return Json (new { isSuccess = true });
				}
				else {
					return Json (new { isSuccess = false });
				}

			}

		}

		public ActionResult Logout()
		{
			FormsAuthentication.SignOut();
			Session.Clear();
			Session.RemoveAll();
			return RedirectToAction("Index", "Account");
		}

		public ActionResult SuccessRegistration(){
			return View ();
		}

		public ActionResult ForgotPasswordRequestSent(){
			return View ();
		}

		private RegisterModelState GetErrorsFromRegisterModelState()
		{
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

		private ForgotPasswordModelState GetErrorsFromForgotPasswordModelState()
		{
			var forgotPasswordModelState = new ForgotPasswordModelState();

			foreach(var key in ModelState.Keys){

				var value = ModelState[key];

				var errorMessage = "";

				foreach (var error in value.Errors) {
					errorMessage += error.ErrorMessage;
				}

				if(key == "Username"){
					forgotPasswordModelState.Username = errorMessage;
				}
				else if(key == "Email"){
					forgotPasswordModelState.Email = errorMessage;
				}

			}

			return forgotPasswordModelState;

		}

	}
}
