using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;

namespace ASP.NET_Console_Project
{
    class ContactView
    {
        private readonly SQLContacts _contacts;

        public ContactView()
        {
            _contacts = new SQLContacts();
        }





        public void DisplayMainMenu()
        {
            int menuOption = 0;
            while (menuOption != 99)
            {
                Console.WriteLine("Main Menu\n");
                Console.WriteLine("1.\tDisplay Contact List");
                Console.WriteLine("2.\tDisplay Contact List Alphabetically");
                Console.WriteLine("99.\tExit");

                Console.WriteLine("\nEnter your Menu option: ");

                try
                {
                    menuOption = Int32.Parse(Console.ReadLine());
                }
                catch (FormatException e)
                {
                    menuOption = 0;
                }
                switch (menuOption)
                {
                    case 1:
                        DisplayContactList();
                        break;
                    case 2:
                        DisplayContactsAlphabetically();
                        break;
                    case 99:
                        break;
                    default:
                        Console.WriteLine("\nInvalid Menu Option\n");
                        break;

                }
            }
        }

        private void DisplayContactList()
        {
            Console.WriteLine("\nContact List");


            List<DataAccessLibrary.BasicContactModel> rows = _contacts.GetBasicContacts();

            if (rows.Count > 0)
            {
                Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,25:S}", "Id", "First Name", "Middle Name", "Last Name");

                foreach (var row in rows)
                {
                    // C# formatted strings
                    // 1. String Interpolation
                    // 2. Console.Writeline("{0} {1} {2}", rows.Id, rows.FirstName, rows.MiddleName, rows.LastName
                    Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,25:S}", row.Id, row.FirstName, row.MiddleName, row.LastName);
                }
            }
            else
            {
                Console.WriteLine("The Contacts Database is empty.");
            }

            int numberOfContacts = _contacts.GetNumberOfContacts();

            if (numberOfContacts > 0)
            {
                // Ternary operator: (1) comparison (2) ? true (3) : false
                Console.WriteLine("\nThere" + ((numberOfContacts == 1) ? " is one Contact " : " are " + numberOfContacts + " Contacts ") + "on file");
            }
            else
            {
                Console.WriteLine("The Contacts Database is empty.");
            }
            Console.WriteLine();

        }

        private void DisplayContactsAlphabetically()
        {
            Console.WriteLine("\nContacts in Alphabetical Order");


            int numberOfContacts = _contacts.GetNumberOfContacts();

            if (numberOfContacts > 0)
            {
                // Ternary operator: (1) comparison (2) ? true (3) : false
                Console.WriteLine("\nThere" + ((numberOfContacts == 1) ? " is one Contact " : " are " + numberOfContacts + " Contacts ") + "on file");
            }
            else
            {
                Console.WriteLine("The Contacts Database is empty.");
            }
            Console.WriteLine();
        }
    }

}
