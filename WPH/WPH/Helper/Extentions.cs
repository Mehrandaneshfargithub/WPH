using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WPH.Helper
{
    public static class Extentions
    {
        public static bool IsValidEmail(this string email)
        {
            try
            {
                var trimmedEmail = email.Trim();

                if (trimmedEmail.EndsWith("."))
                {
                    return false;
                }

                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static int? GetAge(this DateTime? dateOfBirth)
        {
            if (dateOfBirth == null)
                return null;

            return dateOfBirth.Value.GetAge();
        }


        public static int? GetAge(this DateTime dateOfBirth)
        {

            try
            {
                int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
                int dob = int.Parse(dateOfBirth.ToString("yyyyMMdd"));
                return (now - dob) / 10000;
            }
            catch { return null; }
        }

        public static decimal GetDecimalNumber(this string number)
        {
            try
            {
                CultureInfo cultures = new CultureInfo("en-US");

                string re = Regex.Replace(number, @"[^-?\d+\.]", string.Empty);
                return Convert.ToDecimal(re, cultures);
            }
            catch
            {
                return 0;
            }
        }
    }
}
