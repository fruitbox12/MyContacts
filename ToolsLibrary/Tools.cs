using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace ToolsLibrary
{
    class Tools
    {
        public static bool ValidateEmailAddressCharacters(string emailAddress)
        {
            Regex regex = new Regex(@"^[_a-zA-Z0-9-.@]+$");

            return regex.IsMatch(emailAddress);
        }



        public static bool ValidateEmailAddressFormat(string emailAddress)
        {
            Regex regex = new Regex(@"^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*(\.[a-zA-Z]{2,24})$");

            return regex.IsMatch(emailAddress);
        }
        public static string NormalizePhoneNumber(string phoneNumber)
        {
            phoneNumber = Regex.Replace(phoneNumber, @"[^0-9]+", "");


            return phoneNumber;
        }
        public static bool ValidatePhoneNumber(string phoneNumber)
        {
            phoneNumber = Regex.Replace(phoneNumber, @"[^0-9]+", "");


            return phoneNumber.Length == 10;
        }
    }
}
