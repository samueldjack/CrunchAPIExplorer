using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CrunchApiExplorer.Framework.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Encrypts a value using the Data Protection API
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string EncryptAndEncodeAsBase64(this string value)
        {
            var data = Encoding.Unicode.GetBytes(value);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);

            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptBase64EncodedString(this string value)
        {
            byte[] data = Convert.FromBase64String(value);

            //decrypt data
            byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decrypted);
        }

        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
