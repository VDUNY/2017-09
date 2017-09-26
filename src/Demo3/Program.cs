using System;
using System.Security.Cryptography;
using System.Text;

namespace Demo3
{
	class Program
	{
		const string storedHash = "P0EdBW4XR+73cGWfZKDZJ3uz0V9SFbpQqw==";

		static void Main( string[] args )
		{
			using ( var md5 = new MD5CryptoServiceProvider() )
			{
				// convert the stored hash back into a byte array
				byte[] storedHashBytes = Convert.FromBase64String( storedHash );

				// get salt value from stored hash
				int saltSize = storedHashBytes.Length - ( md5.HashSize / 8 );
				byte[] saltBytes = new byte[saltSize];
				Array.Copy( storedHashBytes, 0, saltBytes, 0, saltSize );

				// get the hash value from the stored hash
				byte[] hashBytes = new byte[storedHashBytes.Length - saltSize];
				Array.Copy( storedHashBytes, saltSize, hashBytes, 0, hashBytes.Length );


				// encode message as a byte array
				byte[] messageBytes = Encoding.Unicode.GetBytes( "TEST" );    // TeST

				// combine values
				byte[] saltedMessageBytes = new byte[messageBytes.Length + saltBytes.Length];
				Array.Copy( saltBytes, 0, saltedMessageBytes, 0, saltBytes.Length );
				Array.Copy( messageBytes, 0, saltedMessageBytes, saltBytes.Length, messageBytes.Length );

				// calculate the hash
				byte[] newHashBytes = md5.ComputeHash( messageBytes );
				md5.Clear();

				// compare hash values; if they match then the same 'message' was provided
				bool areSame = CompareBytes( hashBytes, newHashBytes );
				Console.WriteLine( areSame );
				// OUTPUT: true

				Console.ReadKey();
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <remarks>
		/// !!! Faster is not always better !!! Comparing byte arrays in a
		/// short-circuited manner can allow for a side channel timing attack
		/// after multiple attempts. Performing the check against all values
		/// produces a constant time (O() notation) for all attempts.
		/// </remarks>
		/// <param name="a">The first array to compare.</param>
		/// <param name="b">The second array to compare.</param>
		/// <returns>
		/// True if the arrays are equal; false otherwise.
		/// </returns>
		static bool CompareBytes( byte[] a, byte[] b )
		{
			bool result = a.Length == b.Length;

			for ( int i = 0; i < a.Length && i < b.Length; i++ )
			{
				result &= a[i] == b[i];
			}

			return result;
		}
	}
}
