using System;
using System.Security.Cryptography;

namespace Membership.DataAccess
{
	public class PasswordRepository
	{
		static SHA1 _sha;

		public static string GetPassword(string password)
		{
			_sha = new SHA1CryptoServiceProvider();

			byte[] result = _sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

			return Convert.ToBase64String(result);
		}
	}
}

