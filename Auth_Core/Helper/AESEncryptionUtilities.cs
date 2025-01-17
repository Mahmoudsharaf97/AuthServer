﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth_Core.Helper
{
	public static class AESEncryptionUtilities
	{
		//While an app specific salt is not the best practice for
		//password based encryption, it's probably safe enough as long as
		//it is truly uncommon. Also too much work to alter this answer otherwise.
		private static byte[] _salt = Encoding.UTF8.GetBytes("tameenk-sec-key-pass-word#$&");


		/// <summary>
		/// Encrypt the given string using AES.  The string can be decrypted using 
		/// DecryptString().  The sharedSecret parameters must match.
		/// </summary>
		/// <param name="plainText">The text to encrypt.</param>
		/// <param name="sharedSecret">A password used to generate a key for encryption.</param>
		public static string EncryptString(string plainText, string sharedSecret)
		{
			if (string.IsNullOrEmpty(plainText))
				throw new AppException(ExceptionEnum.GenricError);
			//  throw new TameenkArgumentNullException("plainText");
			if (string.IsNullOrEmpty(sharedSecret))
				throw new AppException(ExceptionEnum.GenricError);
			// throw new TameenkArgumentNullException("sharedSecret");

			string outStr = null;                       // Encrypted string to return
			RijndaelManaged aesAlg = null;              // RijndaelManaged object used to encrypt the data.

			try
			{
				// generate the key from the shared secret and the salt
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

				// Create a RijndaelManaged object
				aesAlg = new RijndaelManaged();
				aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

				// Create a decryptor to perform the stream transform.
				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				// Create the streams used for encryption.
				using (MemoryStream msEncrypt = new MemoryStream())
				{
					// prepend the IV
					msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
					msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{
							//Write all data to the stream.
							swEncrypt.Write(plainText);
						}
					}
					outStr = Convert.ToBase64String(msEncrypt.ToArray());
				}
			}

			catch (Exception ex)
			{
				throw new SystemException("Couldn't encrypt the given string.", ex);
			}

			finally
			{
				// Clear the RijndaelManaged object.
				if (aesAlg != null)
					aesAlg.Clear();
			}

			// Return the encrypted bytes from the memory stream.
			return outStr;
		}

		/// <summary>
		/// Decrypt the given string.  Assumes the string was encrypted using 
		/// EncryptStringAES(), using an identical sharedSecret.
		/// </summary>
		/// <param name="cipherText">The text to decrypt.</param>
		/// <param name="sharedSecret">A password used to generate a key for decryption.</param>
		public static string DecryptString(string cipherText, string sharedSecret)
		{
			if (string.IsNullOrEmpty(cipherText))
				throw new AppException(ExceptionEnum.GenricError);

			if (string.IsNullOrEmpty(sharedSecret))

				throw new AppException(ExceptionEnum.GenricError);

			// Declare the RijndaelManaged object
			// used to decrypt the data.
			RijndaelManaged aesAlg = null;

			// Declare the string used to hold
			// the decrypted text.
			string plaintext = null;

			try
			{
				// generate the key from the shared secret and the salt
				Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

				// Create the streams used for decryption.                
				byte[] bytes = Convert.FromBase64String(cipherText);
				using (MemoryStream msDecrypt = new MemoryStream(bytes))
				{
					// Create a RijndaelManaged object
					// with the specified key and IV.
					aesAlg = new RijndaelManaged();
					aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
					// Get the initialization vector from the encrypted stream
					aesAlg.IV = ReadByteArray(msDecrypt);
					// Create a decrytor to perform the stream transform.
					ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))

							// Read the decrypted bytes from the decrypting stream
							// and place them in a string.
							plaintext = srDecrypt.ReadToEnd();
					}
				}
			}

			catch (Exception ex)
			{
				throw new SystemException("Couldn't decrypt the given string.", ex);
			}

			finally
			{
				// Clear the RijndaelManaged object.
				if (aesAlg != null)
					aesAlg.Clear();
			}

			return plaintext;
		}

		private static byte[] ReadByteArray(Stream s)
		{
			byte[] rawLength = new byte[sizeof(int)];
			if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
			{
				throw new SystemException("Stream did not contain properly formatted byte array");
			}

			byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
			if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
			{
				throw new SystemException("Did not read byte array properly");
			}

			return buffer;
		}
	}
}
