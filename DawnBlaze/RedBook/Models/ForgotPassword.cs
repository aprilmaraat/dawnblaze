using System;
using System.ComponentModel.DataAnnotations;

namespace RedBook
{
	public class ForgotPassword
	{
		[Required(ErrorMessage = "Username is required.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }
	}
}

