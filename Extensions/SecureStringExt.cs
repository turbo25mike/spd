using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Spd.Console.Extensions
{
    public static class StringExt
    {
        public static string ConvertToUnsecureString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static SecureString ConvertToSecureString(this string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();

            foreach (var c in password.ToCharArray())
            {
                securePassword.AppendChar(c);
            }

            securePassword.MakeReadOnly();
            return securePassword;
        }

        public static string ToBase64UrlEncoding(this byte[] bytes)
        {
            var padding = new[] { '=' };
            return Convert.ToBase64String(bytes).TrimEnd(padding).Replace('+', '-').Replace('/', '_').Replace("=", "");
        }
    }
}
