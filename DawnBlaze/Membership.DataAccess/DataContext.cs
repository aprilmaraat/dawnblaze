using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Membership.Data;

namespace Membership.DataAccess
{
	public class DataContext : DbContext
	{
		public DataContext()
			: base("name=Membership")
		{ }

		public DbSet<Application> Applications { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserApplication> UserApplications { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

//			#region Person Dependecy
//			modelBuilder.Entity<Person>()
//				.HasRequired(x => x.Gender)
//				.WithMany()
//				.HasForeignKey(x => x.GenderId)
//				.WillCascadeOnDelete(false);
//
//			modelBuilder.Entity<Person>()
//				.HasRequired(x => x.Prefix)
//				.WithMany()
//				.HasForeignKey(x => x.PrefixId)
//				.WillCascadeOnDelete(false);
//
//			modelBuilder.Entity<Person>()
//				.HasRequired(x => x.CivilStatus)
//				.WithMany()
//				.HasForeignKey(x => x.CivilStatusId)
//				.WillCascadeOnDelete(false);
//			#endregion Person Dependecy

		}
	}
}
