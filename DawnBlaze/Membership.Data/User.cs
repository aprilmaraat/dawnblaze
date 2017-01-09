using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Membership.Data
{
	public class User
	{
		public string UserId { get; set; }
		public string UserName {get; set;}
		public string PasswordString {get; set;}
		public string Email {get; set;}
	}
}

