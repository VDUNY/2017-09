using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Demo6
{
	class Program
	{
		const string password = "Sup3rS3cr3tPa$$w0rd!";
		const string message = "Hello VDUNY!";

		static void Main( string[] args )
		{
			var key = GenerateKey( password );
			var iv = GenerateIV( 16 );  // AES is 128-bit
			var messageBytes = Encoding.Unicode.GetBytes( message );

			// encrypt
			byte[] cipherBytes = Encrypt( messageBytes, key, iv );
			var encryptedString = Convert.ToBase64String( cipherBytes );
			Console.WriteLine( "ciphertext: \t{0}\n", encryptedString );

			// decrypt
			byte[] plainBytes = Decrypt( cipherBytes, key, iv );
			var plainString = Encoding.Unicode.GetString( plainBytes );
			Console.WriteLine( "plaintext: \t{0}", plainString );

			Console.ReadKey();
		}

		static byte[] Encrypt( byte[] plainBytes, byte[] key, byte[] iv )
		{
			byte[] cipherBytes;

			using ( var aes = new AesCryptoServiceProvider() )
			{
				//aes.Mode = CipherMode.CBC;		// default
				//aes.Padding = PaddingMode.PKCS7;	// default
				aes.Key = key;
				aes.IV = iv;

				using ( var memoryStream = new MemoryStream() )
				{
					var cryptoStream = new CryptoStream(
							memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write );
					cryptoStream.Write( plainBytes, 0, plainBytes.Length );
					cryptoStream.FlushFinalBlock();

					cipherBytes = memoryStream.ToArray();
				}

				aes.Clear();
			}

			return cipherBytes;
		}

		// decryption
		static byte[] Decrypt( byte[] cipherBytes, byte[] key, byte[] iv )
		{
			byte[] plainBytes;

			using ( var aes = new AesCryptoServiceProvider() )
			{
				//aes.Mode = CipherMode.CBC;		// default
				//aes.Padding = PaddingMode.PKCS7;	// default
				aes.Key = key;
				aes.IV = iv;
				using ( var memoryStream = new MemoryStream() )
				{
					var cryptoStream = new CryptoStream(
							memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write );
					cryptoStream.Write( cipherBytes, 0, cipherBytes.Length );
					cryptoStream.FlushFinalBlock();
					plainBytes = memoryStream.ToArray();
				}

				aes.Clear();
			}

			return plainBytes;
		}

		static byte[] GenerateKey( string password )
		{
			byte[] key;
			using ( var sha256 = new SHA256Cng() )
			{
				var passwordBytes = Encoding.Unicode.GetBytes( password );
				key = sha256.ComputeHash( passwordBytes );
			}

			return key;
		}

		static byte[] GenerateIV( int size )
		{
			var key = new byte[size];
			using ( var rng = new RNGCryptoServiceProvider() )
			{
				rng.GetBytes( key );
			}

			return key;
		}
	}
}
