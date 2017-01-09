using System;
using Membership.Data;

namespace Membership.DataAccess
{
	public class UnitOfWork
	{
		private readonly DataContext _context = new DataContext();

		private GenericRepository<Application> _applicationRepository;
		private GenericRepository<Role> _roleRepository;
		private GenericRepository<User> _userRepository;
		private GenericRepository<UserApplication> _userApplicationRepository;

		public GenericRepository<Application> ApplicationRepository
		{
			get
			{
				return _applicationRepository ?? (_applicationRepository = new GenericRepository<Application>(_context));
			}
		}

		public GenericRepository<Role> RoleRepository
		{
			get
			{
				return _roleRepository ?? (_roleRepository = new GenericRepository<Role>(_context));
			}
		}

		public GenericRepository<User> UserRepository
		{
			get
			{
				return _userRepository ?? (_userRepository = new GenericRepository<User>(_context));
			}
		}

		public GenericRepository<UserApplication> UserApplicationRepository
		{
			get
			{
				return _userApplicationRepository ?? (_userApplicationRepository = new GenericRepository<UserApplication>(_context));
			}
		}

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
