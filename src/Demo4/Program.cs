using System;
using System.Security.Cryptography;
using System.Text;

namespace Demo4
{
	class Program
	{
		const string message = "Hello VDUNY!";

		static void Main( string[] args )
		{
			// generate a key; 32 bytes * 8 bits/byte = 256 bits
			// should be a pre-generated value with high entropy
			var key = GenerateKey();

			// calculate HMAC
			byte[] hash;
			using ( var hmac = new HMACSHA256( key ) )
			{
				var messageBytes = Encoding.Unicode.GetBytes( message );
				hash = hmac.ComputeHash( messageBytes );
				hmac.Clear();
			}

			Console.WriteLine( Convert.ToBase64String( hash ) );
			// SAMPLE OUTPUT: qNW++z2JXsuydyhNm+SKg7XvNv9sAZILXcK2Azp9Bco=

			Console.ReadKey();
		}

		static byte[] GenerateKey()
		{
			var key = new byte[32];
			using ( var rng = new RNGCryptoServiceProvider() )
			{
				rng.GetBytes( key );
			}

			return key;
		}
	}
}
