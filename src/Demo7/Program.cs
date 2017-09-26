using System;
using System.Security.Cryptography;
using System.Text;

namespace Demo7
{
	class Program
	{
		const int KeySize = 2048;
		const string message = "Hello VDUNY!";

		static RSAParameters publicKey;
		static RSAParameters privateKey;

		static void Main( string[] args )
		{
			var plainBytes = Encoding.Unicode.GetBytes( message );

			// build a unique Public/Private Key Pair (in-memory)
			GenerateKeyPair();

			// encrypt with public key
			var encryptedBytes = EncryptData( plainBytes );
			Console.WriteLine( "Encrypted value:  {0}\n",
					Convert.ToBase64String( encryptedBytes ) );

			// decrypt with private key
			var decryptedBytes = DecryptData( encryptedBytes );
			Console.WriteLine( "Decrypted value:  {0}",
					Encoding.Unicode.GetString( decryptedBytes ) );

			Console.ReadKey();
		}

		static void GenerateKeyPair()
		{
			using ( var rsa = new RSACryptoServiceProvider( KeySize ) )
			{
				rsa.PersistKeyInCsp = false;

				publicKey = rsa.ExportParameters( includePrivateParameters: false );
				privateKey = rsa.ExportParameters( includePrivateParameters: true );

				rsa.Clear();
			}
		}

		static byte[] EncryptData( byte[] plainBytes )
		{
			byte[] cipherBytes;

			using ( var rsa = new RSACryptoServiceProvider( KeySize ) )
			{
				rsa.PersistKeyInCsp = false;
				rsa.ImportParameters( publicKey );

				// OAEP padding on Windows XP+; otherwise PKCS#1 v1.5 is used
				cipherBytes = rsa.Encrypt( plainBytes, true );
				rsa.Clear();
			}

			return cipherBytes;
		}

		static byte[] DecryptData( byte[] ciphertext )
		{
			byte[] plainBytes;

			using ( var rsa = new RSACryptoServiceProvider( 2048 ) )
			{
				rsa.PersistKeyInCsp = false;
				rsa.ImportParameters( privateKey );

				plainBytes = rsa.Decrypt( ciphertext, true );
				rsa.Clear();
			}

			return plainBytes;
		}
	}
}
