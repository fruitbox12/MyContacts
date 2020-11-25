using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace ToolsLibrary
{
    public class Tools
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

        public static string InvalidEmailCharacter
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Email Address containts invalid character(s).\n");
                stringBuilder.Append("Valid characters: underscore a-z A-Z 0-9 dash period @");
                return stringBuilder.ToString();
            }
        }
        public static string InvalidEmailFormat
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Invalid Email Address format entered. Valid format:\n");
                stringBuilder.Append("Email userid: at least one character followed by @\n");
                stringBuilder.Append("Email domain name: at least one character followed by period\n");
                stringBuilder.Append("Top-level domain name: at least two characters\n");
                stringBuilder.Append("Minimal example: j@a.co");
                return stringBuilder.ToString();
            }
        }

        public static string InvalidPhoneNumberCharacter
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("PhoneNumber containts invalid character(s).\n");
                stringBuilder.Append("Valid characters: 0-9");
                return stringBuilder.ToString();
            }
        }
        public static string InvalidPhoneNumberFormat
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("Invalid PhoneNumber format entered. Valid format:\n");
                stringBuilder.Append("Email userid: at least one character followed by @\n");
                stringBuilder.Append("Email domain name: at least one character followed by period\n");
                stringBuilder.Append("Top-level domain name: at least two characters\n");
                stringBuilder.Append("Minimal example: 909");
                return stringBuilder.ToString();
            }
        }
    }
}
