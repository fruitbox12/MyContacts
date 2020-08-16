using DataAccessLibrary;
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
            string option = "";
            int menuOption = 0;
            while (menuOption != 99)
            {
                Console.Write("Enter your Menu option: ");
                Console.WriteLine("1.\tDisplay Contact List");
                Console.WriteLine("2.\tDisplay Contact List Alphabetically");
                Console.WriteLine("3.\tDisplay Contact List ID order");

                Console.WriteLine("99.\tExit");

                Console.WriteLine("\nEnter your Menu option: -or- Press Enter to Exit ");

                try
                {
                    option = Console.ReadLine();
                    if (option.Length > 0)
                    {
                        menuOption = Int32.Parse(option);
                    }
                    else
                    {
                        menuOption = 99;
                    }
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

                    case 3:
                       DisplayContactsById();
                        break;
                    case 99:
                        Console.WriteLine("\nExiting \n");
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


            List<BasicContactModel> rows = _contacts.GetBasicContacts();

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

            List<ContactModel> contactModels = _contacts.GetContacts();




            if (contactModels.Count > 0)
            {
                Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,15:S}", "Id", "First Name", "Middle Name", "Last Name", "Email Address");
                foreach (var contactModel in contactModels)
                {
                    Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,15:S}" , contactModel.BasicContactModel.Id, contactModel.BasicContactModel.FirstName,
                        contactModel.BasicContactModel.MiddleName, contactModel.BasicContactModel.LastName);

                    if (contactModel.EmailAddress.Count > 0)
                    {
                        Console.WriteLine($"\n\t\t EmailAddresses \n");

                        foreach (var emailAddressModel in contactModel.EmailAddress)
                        {
                            Console.WriteLine($"\t\t { emailAddressModel.EmailAddress} ");
                        }
                    }

                    if (contactModel.PhoneNumber.Count > 0)
                    {
                        Console.WriteLine($"\n\t\t Phone Numbers \n");

                        foreach (var phoneNumberModel in contactModel.PhoneNumber)
                        {
                            Console.WriteLine($"\t\t{ phoneNumberModel.PhoneNumber}");
                            Console.WriteLine();



                        }
                        Console.WriteLine();

                    }

                }
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
        private void DisplayContactsById()
        {
            Console.WriteLine("\nContacts in Id Order\n");

            List<ContactModel> contactModels = _contacts.GetContactsById();
            foreach(var contactModel in contactModels)
            {
                Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,15:S}", contactModel.BasicContactModel.Id, contactModel.BasicContactModel.FirstName,
                        contactModel.BasicContactModel.MiddleName, contactModel.BasicContactModel.LastName);
                Console.WriteLine();


            }
        }

    }


}