using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Membership.Data
{
	public class Application
	{
		public string ApplicationId {get; set;}
		public string ApplicationName {get; set;}
		public bool IsActive { get; set;}
	}
}

