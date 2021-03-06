﻿using System;
using System.ComponentModel.DataAnnotations;

namespace RedBook
{
	public class Register
	{
		[Required(ErrorMessage = "Username is required.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		[EmailAddress(ErrorMessage = "Invalid Email Address")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password must not be empty.")]
		public string ConfirmPassword { get; set; }
	}
}

