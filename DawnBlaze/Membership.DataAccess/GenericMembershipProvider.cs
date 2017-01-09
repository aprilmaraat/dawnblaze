using System;
using System.Web.Security;
using Membership.Data;

namespace Membership.DataAccess
{
	public class GenericMembershipProvider : MembershipProvider
	{

		private readonly UnitOfWork _unitOfWork;

		public GenericMembershipProvider()
		{
			_unitOfWork = new UnitOfWork();
		}

		public override int MinRequiredPasswordLength
		{
			get { return 5; }
		}

		public override bool ValidateUser(string userName, string password)
		{
			if (string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(userName.Trim()))
				return false;

			password = PasswordRepository.GetPassword(password);
			return _unitOfWork.UserRepository.GetOne(user => (user.UserName == userName.Trim()) && (user.PasswordString == password)) != null;
		}

		public User GetUser(string userName, string password)
		{
			if (string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(userName.Trim()))
				return null;

			password = PasswordRepository.GetPassword(password);
			return _unitOfWork.UserRepository.GetOne(user => (user.UserName == userName.Trim()) && (user.PasswordString == password));
		}

		public bool CreateUser(Guid userId, string userName, string email)
		{
			var newUser = new User
			{
				UserId = userId.ToString(),
				UserName = userName,
				PasswordString = PasswordRepository.GetPassword(userName).ToString(),
				Email = email
			};
			try
			{
				_unitOfWork.UserRepository.Insert(newUser);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public override bool ChangePassword(string userName, string oldPassword, string newPassword)
		{
			if (!ValidateUser(userName, oldPassword) || string.IsNullOrEmpty(newPassword.Trim()))
				return false;

			try
			{
				User user = _unitOfWork.UserRepository.GetOne(u => u.UserName == userName);
				user.PasswordString = PasswordRepository.GetPassword(newPassword.Trim());
				_unitOfWork.UserRepository.Update(user);
				return true;
			}
			catch { return false; }

		}

		#region Not Implemented MembershipProvider Methods

		#region Properties

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override string ApplicationName
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int MaxInvalidPasswordAttempts
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int MinRequiredNonAlphanumericCharacters
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int PasswordAttemptWindow
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override MembershipPasswordFormat PasswordFormat
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override string PasswordStrengthRegularExpression
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool RequiresQuestionAndAnswer
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool RequiresUniqueEmail
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool EnablePasswordReset
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool EnablePasswordRetrieval
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override int GetNumberOfUsersOnline()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override string GetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override string GetUserNameByEmail(string email)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override string ResetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This function is not implemented.
		/// </summary>
		public override bool UnlockUser(string userName)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// This method is not implemented.
		/// </summary>
		public override void UpdateUser(MembershipUser user) 
		{
			throw new NotImplementedException();
		}

		#endregion

	}
}

