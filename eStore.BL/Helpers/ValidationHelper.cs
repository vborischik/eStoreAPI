using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace eStore.BL.Helpers
{
    public static class ValidationHelper
    {

        /// <summary>
        /// Validates the provided email address.
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // The MailAddress class will throw an exception if the email is not valid.
                var addr = new MailAddress(email);
                return addr.Address.Equals(email, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Validates the provided phone number.
        /// For this example, we assume a valid phone is 10-15 digits, and can optionally start with a '+'.
        /// Adjust the regex pattern as needed for your scenario.
        /// </summary>
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;
            phone = phone.Replace("-", "");
            var regex = new Regex(@"^\+?\d{10,15}$");
            return regex.IsMatch(phone);
        }


    }
}
