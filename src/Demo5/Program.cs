using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Demo5
{
	class Program
	{
		const string password = "Sup3rS3cr3tPa$$w0rd!";
		const int HashSize = 32;    // # bytes to represent password; 256-bits min. rec.

		static void Main( string[] args )
		{
			// generate a salt value; 32 bytes * 8 bits/byte = 256 bits
			var salt = GenerateSalt();
			var passwordBytes = Encoding.Unicode.GetBytes( password );

			// show timings; 50,000 min. rec.
			for ( int i = 10000; i <= 200000; i += 10000 )
			{
				HashPassword( passwordBytes, salt, i );
			}

			Console.ReadKey();
		}

		static void HashPassword( byte[] password, byte[] salt, int iterations )
		{
			var sw = new Stopwatch();
			byte[] hash;

			sw.Start();
			using ( var pbkdf = new Rfc2898DeriveBytes( password, salt, iterations ) )
			{
				hash = pbkdf.GetBytes( HashSize );
			}
			sw.Stop();

			Console.WriteLine( "Iterations: {00} ({1}ms)\t : \t{2}",
					iterations,
					sw.ElapsedMilliseconds,
					Convert.ToBase64String( hash ) );
		}

		// generate a random key
		static byte[] GenerateSalt()
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
