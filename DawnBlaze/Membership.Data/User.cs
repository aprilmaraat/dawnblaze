﻿
namespace Membership.Data
{
	public class User
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string PasswordString { get; set; }
		public string Email { get; set; }
		public int IsConfirmed { get; set; }
		public int LoginAttempts { get; set; }
		public int IsLockedOut { get; set; }
	}
}

