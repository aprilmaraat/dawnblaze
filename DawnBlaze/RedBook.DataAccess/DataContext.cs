using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using RedBook.Data;

namespace RedBook.DataAccess
{
	public class DataContext : DbContext
	{
		public DataContext()
			: base("name=Membership")
		{ }

//		public DbSet<Role> Roles { get; set; }

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
