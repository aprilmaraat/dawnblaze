using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Membership.Data
{
	public class User
	{
		[Key]
		[DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }

		public string UserName { get; set; }
		public string PasswordString { get; set; }
		public string Email { get; set; }
		public int IsConfirmed { get; set; }
		public int LoginAttempts { get; set; }
		public int IsLockedOut { get; set; }
	}
}

