using System.Text;
using System;

namespace VladB.Utility {
    public static partial class Extensions {
        public static bool IsNullOrEmpty(this string s) => string.IsNullOrEmpty(s);
        public static bool IsHaveSomething(this string s) => !string.IsNullOrEmpty(s);

        public static string GetRandomPpKey(int length = 20) => GetRandomGuid(length);

        public static string GetRandomGuid(int length = 20) {
            string s1 = Guid.NewGuid().ToString();
            byte[] s2 = Encoding.UTF8.GetBytes(s1);
            string s3 = Convert.ToBase64String(s2);
            return s3.Substring(0, length);
        }
    }
}

