using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DotNetCoreConsole
{
    public class TestGeneratePassword
    {
        // Still in progress.
        public static void TestMain()
        {
            string password = GenerateStrongUserPassword();
            System.Console.WriteLine(password);
        }

        private static string GetJsonProperty(string jsonStr)
        {
            return Regex.Replace(jsonStr, "[^\\w_.]+", "_", RegexOptions.Compiled);
        }

        /// <summary>
        /// Generates a strong user password.
        /// </summary>
        /// <returns>Generated password.</returns>
        private static string GenerateStrongUserPassword()
        {
            return GeneratePassword(
                16,
                "abcdefghijklmnopqrstuvwxyz",
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ",
                "23456789",
                ".-_!#^~");
        }

        /// <summary>
        /// Generates a random password of specified length having at least one character from each of specified sets.
        /// If requested password length is less than number of character sets, only those many sets are guaranteed
        /// to have one of their characters in the password.
        /// </summary>
        /// <param name="passwordLength">Length of desired password.</param>
        /// <param name="allowedCharacterSets">Allowed character sets.</param>
        /// <returns>Random generated password.</returns>
        private static string GeneratePassword(int passwordLength, params string[] allowedCharacterSets)
        {
            char[] passwordArray = new char[passwordLength];

            using (RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider())
            {
                byte[] randomData = new byte[passwordLength];
                provider.GetBytes(randomData);
                for (int i = 0; i < passwordLength; i++)
                {
                    IList<char> set = allowedCharacterSets[i % allowedCharacterSets.Length].ToList<char>();
                    passwordArray[i] = set[randomData[i] % set.Count];
                }

                return new string(passwordArray);
            }
        }
    }
}
