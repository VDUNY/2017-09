using System;
using System.Security.Cryptography;
using System.Text;

namespace Demo2
{
	class Program
	{
		static void Main( string[] args )
		{
			// encode message as a byte array
			byte[] messageBytes = Encoding.Unicode.GetBytes( "TEST" );

			// get salt value
			byte[] saltBytes = GenerateRandomSalt();

			// combine values
			byte[] saltedMessageBytes = new byte[messageBytes.Length + saltBytes.Length];
			Array.Copy( saltBytes, 0, saltedMessageBytes, 0, saltBytes.Length );
			Array.Copy( messageBytes, 0, saltedMessageBytes, saltBytes.Length, messageBytes.Length );

			// calculate the hash
			byte[] hashBytes;
			using ( var md5 = new MD5CryptoServiceProvider() )
			{
				hashBytes = md5.ComputeHash( messageBytes );
				md5.Clear();
			}

			// combine salt & hash
			byte[] saltedHashBytes = new byte[saltBytes.Length + hashBytes.Length];
			Array.Copy( saltBytes, 0, saltedHashBytes, 0, saltBytes.Length );
			Array.Copy( hashBytes, 0, saltedHashBytes, saltBytes.Length, hashBytes.Length );

			// output as Base64-encoded string
			var output = Convert.ToBase64String( saltedHashBytes );
			Console.Write( output );
			// SAMPLE OUTPUT: P0EdBW4XR+73cGWfZKDZJ3uz0V9SFbpQqw==

			Console.ReadKey();
		}

		/// <summary>Builds a random 'salt' value.</summary>
		/// <returns>Random byte array.</returns>
		static byte[] GenerateRandomSalt()
		{
			// use basic Random to determine size
			// short for demo; should be larger in practice for more entropy
			var rand = new Random();
			var size = rand.Next( 8, 12 );
			var saltBytes = new byte[size];

			// use cryptographically secure random
			using ( var cryptoRand = new RNGCryptoServiceProvider() )
			{
				cryptoRand.GetBytes( saltBytes );
			}

			return saltBytes;
		}
	}
}
