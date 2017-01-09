using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Membership.Data
{
	public class Role
	{
		public string RoleId { get; set; }
		public string RoleName { get; set; }
		public bool IsActive { get; set;}

	}
}

