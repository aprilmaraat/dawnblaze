using System;

namespace RedBook.DataAccess
{
	public class UnitOfWork
	{
		private readonly DataContext _context = new DataContext();

//		private GenericRepository<Role> _roleRepository;
//
//		public GenericRepository<Role> RoleRepository
//		{
//			get
//			{
//				return _roleRepository ?? (_roleRepository = new GenericRepository<Role>(_context));
//			}
//		}

		public void Save()
		{
			_context.SaveChanges();
		}

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
			}
			_disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
