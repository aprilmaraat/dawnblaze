using System;

namespace Membership.Data
{
	public class ActivityLog
	{
		public string ActivityLogId { get; set; }
		public string ActivityName { get; set; }
		public string UserId { get; set; }
		public DateTime ActivityDate { get; set; }
	}
}

