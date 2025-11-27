using System.Security.Cryptography;

namespace SchoolAPI.Services
{
	public class PasswordHasher
	{
		public static string HashPassword(string password)
		{
			// Generate a salt
			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

			// Create the PBKDF2 (Password-Based Key Derivation Function 2)
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
			byte[] hash = pbkdf2.GetBytes(20);

			// Combine the salt and password bytes
			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);

			// Convert to base64
			return Convert.ToBase64String(hashBytes);
		}

		public static bool VerifyPassword(string password, string hashedPassword)
		{
			// Extract the bytes
			byte[] hashBytes = Convert.FromBase64String(hashedPassword);

			// Get the salt
			byte[] salt = new byte[16];
			Array.Copy(hashBytes, 0, salt, 0, 16);

			// Compute the hash on the password the user entered
			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
			byte[] hash = pbkdf2.GetBytes(20);

			// Compare the results
			for (int i = 0; i < 20; i++)
			{
				if (hashBytes[i + 16] != hash[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}