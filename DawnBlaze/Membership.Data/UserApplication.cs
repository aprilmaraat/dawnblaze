using System;

namespace Membership.Data
{
	public class UserApplication
	{
		public string UserApplicationId { get; set; }
		public string ApplicationId { get; set; }
		public string UserId { get; set; }
		public string RoleId { get; set; }
		public bool IsConfirmed { get; set; }
		public int AccessFailedCount { get; set; }
		public bool IsLockedOut { get; set; }
		public DateTime LockedOutDate { get; set; }
		public bool IsActive { get; set; }
	}
}

