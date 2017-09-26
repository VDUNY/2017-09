using System;
using System.Security.Cryptography;
using System.Text;

namespace Demo1
{
	class Program
	{
		static void Main( string[] args )
		{
			// encode message as a byte array
			var messageBytes = Encoding.Unicode.GetBytes( "TEST" );

			// calculate the hash
			byte[] hashBytes;
			using ( var md5 = new MD5CryptoServiceProvider() )
			{
				hashBytes = md5.ComputeHash( messageBytes );
				md5.Clear();
			}

			// output as a hex string
			for ( int i = 0; i < hashBytes.Length; i++ )
			{
				Console.Write( hashBytes[i].ToString( "x2" ) );
			}
			// OUTPUT: 70659f64a0d9277bb3d15f5215ba50ab

			Console.ReadKey();
		}
	}
}