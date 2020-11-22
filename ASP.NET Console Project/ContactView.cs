using DataAccessLibrary;
using SQLContactsLibrary;
using SQLContactsLibrary.Models;
using System;
using Settings;
using System.Collections.Generic;
using ToolsLibrary;
namespace ASP.NET_Console_Project
{
    public class ContactView
    {
        private readonly IContacts _contacts;
        private readonly IAppSettings _appSettings;


        //using de[emdemcy injection (DI) to recieve expected
        public ContactView(IAppSettings appSettings, IContacts contacts)
        {
            _contacts = contacts;
        }

        public void DisplayMainMenu()
        {
            string option = "";
            int menuOption = 0;
            while (menuOption != 99)
            {
                Console.WriteLine("Main Menu\n");
                Console.WriteLine("1.\tDisplay Contact List");
                Console.WriteLine("2.\tDisplay Contact List Alphabetically");
                Console.WriteLine("3.\tDisplay Contact List ID order");
                Console.WriteLine("4.\tSearch Contacts List by id");
                Console.WriteLine("5.\tSearch Contacts by Name");
                Console.WriteLine("6.\tSearch Contacts by Email");
                Console.WriteLine("7.\tSearch Contacts by Phone");
                Console.WriteLine("8.\tAdd contact");
                Console.Write("\nEnter your Menu option: -or- Press Enter to Exit ");

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
                        DisplayContactsByName();
                        break;

                    case 3:
                        DisplayContactsById();
                        break;
                    case 4:
                        SearchContactsById();
                        break;
                    case 5:
                        SearchContactByName();
                        break;
                    case 6:
                        SearchContactsByEmail();
                        break;
                    case 7:
                        SearchContactsByPhoneNumber();
                        break;
                    case 8:
                        addContact();
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


            ResultSet<List<BasicContactModel>> resultSet = _contacts.GetBasicContactsByName();
            //removed if statement below, basicContactModels is already greater than 0,
            if (resultSet.Result.Count > 0)
            {
                Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,25:S}", "Id", "First Name", "Middle Name", "Last Name");
                foreach (var basicContactModel in resultSet.Result)
                {
                    // C# formatted strings
                    // 1. String Interpolation
                    // 2. Console.Writeline("{0} {1} {2}", rows.Id, rows.FirstName, rows.MiddleName, rows.LastName
                    Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,25:S}", basicContactModel.Id, basicContactModel.FirstName, basicContactModel.MiddleName, basicContactModel.LastName);

                }
                Console.WriteLine("\nThere" + ((resultSet.Result.Count == 1) ? " is one Contact " : " are " + resultSet.Result.Count + " Contacts ") + "on file");

            }

            else if (resultSet.CriticalError || resultSet.LogicalError)
            {
                Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                foreach (Trace trace in resultSet.Traces)
                {
                 
                    Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}", 
                        trace.ErrorType.ToString(),
                        trace.ClassName,
                        trace.MemberName,
                        trace.ErrorNumber);
                    Console.WriteLine("Error Messages:");
                    foreach (string errorMessage in trace.ErrorMessages)
                    {
                        Console.WriteLine(errorMessage);

                    }
                }
                
            }
            else
            {
                Console.WriteLine("The Contacts Database is empty.");
            }
            ResultSet<int> numberOfContacts = _contacts.GetNumberOfContacts();

            Console.WriteLine();
        }

        private void DisplayContactsByName()
        {
            Console.WriteLine("\nContacts in Alphabetical Order");
            ResultSet<List<ContactModel>> resultSet = _contacts.GetContactsByName();

            if (resultSet.Result.Count > 0)
            {
                Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,15:S}", "Id", "First Name", "Middle Name", "Last Name", "Email Address");
                foreach (var contactModel in resultSet.Result)
                {
                    Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,15:S}", contactModel.BasicContactModel.Id,
                                                                               contactModel.BasicContactModel.FirstName,
                                                                               contactModel.BasicContactModel.MiddleName,
                                                                               contactModel.BasicContactModel.LastName);

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

                    else if (resultSet.CriticalError || resultSet.LogicalError)
                    {
                        Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                        foreach (Trace trace in resultSet.Traces)
                        {

                            Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                                trace.ErrorType.ToString(),
                                trace.ClassName,
                                trace.MemberName,
                                trace.ErrorNumber);
                            Console.WriteLine("Error Messages:");
                            foreach (string errorMessage in trace.ErrorMessages)
                            {
                                Console.WriteLine(errorMessage);

                            }
                        }

                    }
                    else
                    {
                        Console.WriteLine("The Contacts Database is empty.");
                    }
                    ResultSet<int> numberOfContacts = _contacts.GetNumberOfContacts();

                    Console.WriteLine();
                }
                

            }
            else if (resultSet.CriticalError || resultSet.LogicalError)
            {
                Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                foreach (Trace trace in resultSet.Traces)
                {

                    Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                        trace.ErrorType.ToString(),
                        trace.ClassName,
                        trace.MemberName,
                        trace.ErrorNumber);
                    Console.WriteLine("Error Messages:");
                    foreach (string errorMessage in trace.ErrorMessages)
                    {
                        Console.WriteLine(errorMessage);

                    }
                }

            }

            else
            {
                Console.WriteLine("\nThere" + ((resultSet.Result.Count == 1) ? " is one Contact " : " are " + resultSet.Result.Count + " Contacts ") + "on file");

            }


        }

        
        private void DisplayContactsById()
        {
            Console.WriteLine("\nContacts in Id Order\n");

            ResultSet<List<ContactModel>> resultSet = _contacts.GetContactsById();
            if (resultSet.Result.Count > 0)
            {
                Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,25:S}", "Id", "First Name", "Middle Name", "Last Name");
                Console.WriteLine();
                foreach (var contactModel in resultSet.Result)
                {


                    Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,25:S}",
                                        contactModel.BasicContactModel.Id,
                                        contactModel.BasicContactModel.FirstName,
                                        contactModel.BasicContactModel.MiddleName,
                                        contactModel.BasicContactModel.LastName);

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
            else if (resultSet.CriticalError || resultSet.LogicalError)
            {
                Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                foreach (Trace trace in resultSet.Traces)
                {

                    Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                        trace.ErrorType.ToString(),
                        trace.ClassName,
                        trace.MemberName,
                        trace.ErrorNumber);
                    Console.WriteLine("Error Messages:");
                    foreach (string errorMessage in trace.ErrorMessages)
                    {
                        Console.WriteLine(errorMessage);

                    }
                }

            }

            else
            {
                Console.WriteLine("The Contacts Database is empty.");

            }
            Console.WriteLine();

        }

        private void SearchContactsById()
        {
            Console.WriteLine("\nSearch Contacts By Id\n");
            int contactId = 0;
            bool finished = false;
            while (!finished)
            {
                try
                {
                    Console.Write("Enter the Contact Id -or- press enter to exit: ");
                    String option = Console.ReadLine();
                    if (option.Length > 0)
                    {
                        contactId = Int32.Parse(option);
                    }
                    else
                    {
                        contactId = 0;
                        finished = true;
                    }
                }

                catch (FormatException e)
                {
                    Console.WriteLine("\nPlease enter a Contact Id number\n");
                    contactId = 0;

                }
                catch (OverflowException e)
                {
                    Console.WriteLine("\nPlease enter a valid Contact Id number\n");
                    contactId = 0;

                }
                if (contactId > 0)
                {
                   ResultSet< BasicContactModel> resultSet = _contacts.GetBasicContactById(contactId);
                    if (resultSet.Result.Id > 0)
                    {
                        Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,25:S}", "Id", "First Name", "Middle Name", "Last Name");
                        Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,25:S}",
                                          resultSet.Result.Id,
                                          resultSet.Result.FirstName,
                                          resultSet.Result.MiddleName,
                                          resultSet.Result.LastName);
                        Console.WriteLine();

                    }
                    else if (resultSet.CriticalError || resultSet.LogicalError)
                    {
                        Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                        foreach (Trace trace in resultSet.Traces)
                        {

                            Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                                trace.ErrorType.ToString(),
                                trace.ClassName,
                                trace.MemberName,
                                trace.ErrorNumber);
                            Console.WriteLine("Error Messages:");
                            foreach (string errorMessage in trace.ErrorMessages)
                            {
                                Console.WriteLine(errorMessage);

                            }

                            Console.WriteLine();
                        }

                    }


                    else
                    {
                        Console.WriteLine("\nContact with Id " + contactId + " not found\n");
                    }



           

                }

            }
            Console.WriteLine();

        }

        private void SearchContactByName()
        {
            Console.WriteLine("\nSearch Contacts By Name\n");

            string firstName = "";
            string lastName = "";

            bool finished = false;

            while (!finished)
            {
                Console.Write("Enter the Contact's First Name Press Enter Twice To Exit: ");
                firstName = Console.ReadLine();

                Console.WriteLine();

                Console.Write("Enter The Contact's Last Name: ");
                lastName = Console.ReadLine();


                if (firstName.Length > 0 || lastName.Length > 0)
                {
                    ResultSet<List<BasicContactModel>> resultSet = _contacts.SearchBasicContactsByName(firstName, lastName);
                    Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,15:S}",
                                         "Id",
                                         "First Name",
                                         "Middle Name",
                                         "Last Name");
                    if (resultSet.Result.Count > 0)
                    {
                        foreach (var basicContactModel in resultSet.Result)
                        {
                            Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,15:S}", basicContactModel.Id, basicContactModel.FirstName,
                                   basicContactModel.MiddleName, basicContactModel.LastName);

                        }
                    }

                    else if (resultSet.CriticalError || resultSet.LogicalError)
                    {
                        Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                        foreach (Trace trace in resultSet.Traces)
                        {

                            Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                                trace.ErrorType.ToString(),
                                trace.ClassName,
                                trace.MemberName,
                                trace.ErrorNumber);
                            Console.WriteLine("Error Messages:");
                            foreach (string errorMessage in trace.ErrorMessages)
                            {
                                Console.WriteLine(errorMessage);

                            }

                            Console.WriteLine();
                        }

                    }

                    else
                    {
                        Console.WriteLine("\nContact with First Name: " + firstName + " and Last Name: " + lastName + " Not Found\n");
                    }
                    Console.WriteLine();


                }
                else
                {
                    finished = true;
                }
            }
        }
        private void SearchContactsByEmail()
        {
            Console.WriteLine("\nSearch A Contact By Email\n");

            string email = "";

            bool finished = false;

            while (!finished)
            {
                Console.Write("Enter the Contact's Email Press Enter To Exit: ");
                email = Console.ReadLine();

                Console.WriteLine();
                if (email.Length > 0)
                {
                    ResultSet<List<EmailAddressModel>> emailResultSet = _contacts.SearchEmail(email);
                    Console.WriteLine("{0, -5:D} {1,15:S} {2,15:S} {3,10:S} {4, -50:S}", "Email Address Id", "First Name", "Middle Name", "Last Name", "Email Address");

                    if (emailResultSet.Result.Count > 0)
                    {
                        foreach (var emailAddress in emailResultSet.Result)
                        {
                            ResultSet<BasicContactModel> basicContactResultSet = _contacts.GetBasicContactByEmail(emailAddress.Id);
                            if (basicContactResultSet.Result.Id > 0)
                            {

                                Console.WriteLine("{0, -5:D} {1,26:S} {2,15:S} {3,10:S} {4, -40}", basicContactResultSet.Result.Id,
                                                                                               basicContactResultSet.Result.FirstName,
                                                                                               basicContactResultSet.Result.MiddleName,
                                                                                               basicContactResultSet.Result.LastName,
                                                                                               emailAddress.EmailAddress);
                            }

                            else if (basicContactResultSet.CriticalError || basicContactResultSet.LogicalError)
                            {
                                Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                                foreach (Trace trace in basicContactResultSet.Traces)
                                {

                                    Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                                        trace.ErrorType.ToString(),
                                        trace.ClassName,
                                        trace.MemberName,
                                        trace.ErrorNumber);
                                    Console.WriteLine("Error Messages:");
                                    foreach (string errorMessage in trace.ErrorMessages)
                                    {
                                        Console.WriteLine(errorMessage);

                                    }

                                    Console.WriteLine();
                                }

                            }
                            else
                            {
                                    Console.WriteLine("\n{0,50:S}\n", emailAddress.EmailAddress);

                                    foreach (Trace trace in emailResultSet.Traces)
                                    {
                                        Console.WriteLine("Class Name:", trace.ClassName);
                                        Console.WriteLine("Member Name:", trace.MemberName);

                                        if (trace.ErrorNumbers > 0)
                                        {
                                            Console.WriteLine(trace.ErrorNumbers);
                                        }
                                        Console.WriteLine(trace.ErrorMessages);

                                    }
                             }
                            
                        }
                    }
                    else if (emailResultSet.CriticalError || emailResultSet.LogicalError)
                    {
                        Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                        foreach (Trace trace in emailResultSet.Traces)
                        {

                            Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                                trace.ErrorType.ToString(),
                                trace.ClassName,
                                trace.MemberName,
                                trace.ErrorNumber);
                            Console.WriteLine("Error Messages:");
                            foreach (string errorMessage in trace.ErrorMessages)
                            {
                                Console.WriteLine(errorMessage);

                            }

                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nContact with Email Address: " + email + " Not Found\n");
                    }
                    Console.WriteLine();

                }
                else
                {
                    finished = true;
                }
            }
        }
        private void SearchContactsByPhoneNumber()  
        {
            Console.WriteLine("\nSearch Contacts By Phone Number\n");
            string number;
            bool finished = false;
            while (!finished)
            {
                Console.Write("Enter the Phone Number -or- press enter to exit: ");
                number = Console.ReadLine();
                if (number.Length > 0)
                {
                    ResultSet<List<PhoneNumberModel>> phoneResultSet = _contacts.SearchPhoneNumbers(number);
                    Console.WriteLine("{0, -10:D} {1,15:S} {2,15:S} {3,10:S} {4, -40:S}", "Phone Number Id", "First Name", "Middle Name", "Last Name", "Phone Number");
                    if (phoneResultSet.Result.Count > 0)
                    {
                        foreach (var phoneNumberModel in phoneResultSet.Result)
                        {
                            ResultSet<BasicContactModel> basicContactResultSet = _contacts.GetBasicContactByEmail(phoneNumberModel.Id);
                            if (basicContactResultSet.Result.Id > 0)
                            {
                                Console.WriteLine("{0, -4:D} {1,26:S} {2,15:S} {3,10:S} {4, -40}",  phoneNumberModel.Id, 
                                                                                                    basicContactResultSet.Result.FirstName,
                                                                                                    basicContactResultSet.Result.MiddleName,
                                                                                                    basicContactResultSet.Result.LastName,
                                                                                                    phoneNumberModel.PhoneNumber);
                            }
                            else if (basicContactResultSet.CriticalError || basicContactResultSet.LogicalError)
                            {
                                Console.WriteLine("{0, -15:D} {1,15:S} {2,15:S} {3,25:S}", "Error Type", "Class Name", "Member Name", "Error Number");

                                foreach (Trace trace in basicContactResultSet.Traces)
                                {

                                    Console.WriteLine("{0, -5:S} {1,15:S} {2,15:S} {3,25:D}", trace.ErrorType.ToString(), trace.ClassName, trace.MemberName, trace.ErrorNumber);
                                    Console.WriteLine("Error Messages:\n");
                                    foreach (string errorMessage in trace.ErrorMessages)
                                    {
                                        Console.WriteLine(errorMessage);
                                    }
                                }
                            }
                        }
                        
                    }
                    else if (phoneResultSet.CriticalError || phoneResultSet.LogicalError)
                    {
                        Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                        foreach (Trace trace in phoneResultSet.Traces)
                        {

                            Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                                trace.ErrorType.ToString(),
                                trace.ClassName,
                                trace.MemberName,
                                trace.ErrorNumber);
                            Console.WriteLine("Error Messages:");
                            foreach (string errorMessage in trace.ErrorMessages)
                            {
                                Console.WriteLine(errorMessage);

                            }

                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nContact with Phone Number: " + number + " Not Found\n");
                    }
                    Console.WriteLine();

                }
              
                else
                {
                    finished = true;
                }
            }
        }
        private void addContact()
        {
            int contactId = AddBasicContact().Result.Id;
            if (contactId > 0)
            {
                AddEmailAddresses(contactId);
            }
           
            //Add Basic contacts
            //Add EmailAddress(s) if Basic contact was successfully added
            //Add Phone Numbers(s)
        }

        private int AddBasicContact2()
        {
            BasicContactModel basicContactModel = new BasicContactModel();

            //input basic contact model
            Console.WriteLine("\nAdd Basic Contact\n");

            string firstName = "";
            string middleName = "";
            string lastName = "";
            bool finished = false;

            while (!finished)
            {
                Console.Write("Enter the Contact's First name ");
                firstName = Console.ReadLine();
                Console.WriteLine();
                Console.Write("Enter The Contact's Middle Name: ");
                middleName = Console.ReadLine();
                Console.WriteLine();
                Console.Write("Enter The Contact's Last Name: ");
                lastName = Console.ReadLine();
                Console.WriteLine();

                if (firstName.Length > 0 && lastName.Length > 0)
                {
                    basicContactModel.FirstName = firstName;
                    basicContactModel.MiddleName = middleName;
                    basicContactModel.LastName = lastName;
                    ResultSet<BasicContactModel> resultSet = _contacts.AddBasicContacts(basicContactModel);

                    if (resultSet.CriticalError || resultSet.LogicalError)
                    {
                        Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                        foreach (Trace trace in resultSet.Traces)
                        {

                            Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                                trace.ErrorType.ToString(),
                                trace.ClassName,
                                trace.MemberName,
                                trace.ErrorNumber);
                            Console.WriteLine("Error Messages:");
                            foreach (string errorMessage in trace.ErrorMessages)
                            {
                                Console.WriteLine(errorMessage);
                            }
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S}", "First Name", "Middle Name", "Last Name");

                        Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S}",
                            firstName,
                            middleName,
                            lastName);
                    }
                    Console.WriteLine();

                    finished = true;
                }
                else if (firstName.Length == 0 && lastName.Length == 0)
                {
                    finished = true;
                }
                //input basic contact model


            }
            return 0;
        }

        private ResultSet<BasicContactModel> AddBasicContact()
        {
            Console.WriteLine("\nAdd Basic Contact\n");
            BasicContactModel basicContact = new BasicContactModel();
            ResultSet<BasicContactModel> resultSet = new ResultSet<BasicContactModel>();
            string firstName = "";
            string middleName = "";
            string lastName = "";
            string firstNameSave = "";
            string middleNameSave = "";
            string lastNameSave = "";
            string answer = "";

            bool finished = false;
            bool firstNameEntered = false;
            bool middleNameEntered = false;
            bool lastNameEntered = false;

            // Ask user before updating the table to confirm the data entered - or - re-enter the data
            do
            {
                if (firstNameEntered)
                {
                    Console.Write("\n The First Name Is '"
                        + firstName
                        + "'. Would You Like To Change It? (Y Or N, Press Enter for no): ");
                    answer = Console.ReadLine().Trim();

                    if (answer.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                        answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Enter The Contact's First Name");
                        Console.WriteLine("Press Spacebar Enter To Erase or Press Enter To Leave Unchanged: ");

                        firstName = Console.ReadLine();

                        // Check for the new first name or erased first name
                        if (firstName.Length > 0)
                        {
                            firstName = firstName.Trim();

                            if (firstName.Length == 0)
                            {
                                firstNameEntered = false;
                            }

                            firstNameSave = firstName;
                        }
                        else
                        {
                            // First Name Unchanged
                            firstName = firstNameSave;
                        }

                    }
                }

                else
                {
                    Console.Write("Enter The Contact's First Name or Press Enter To Leave It Blank: ");
                    firstName = Console.ReadLine().Trim();

                    if (firstName.Length > 0)
                    {
                        firstNameEntered = true;
                    }
                }

                Console.WriteLine("First Name Is Now: " + (firstNameEntered ? "'" + firstName + "'." : "blank."));

                if (middleNameEntered)
                {
                    Console.Write("\n The Middle Name Is '"
                        + middleName
                        + "'. Would You Like To Change It? (Y Or N, Press Enter for no): ");
                    answer = Console.ReadLine().Trim();

                    if (answer.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                        answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Enter The Contact's Middle Name");
                        Console.WriteLine("Press Spacebar Enter To Erase or Press Enter To Leave Unchanged: ");

                        middleName = Console.ReadLine();

                        // Check for the new first name or erased middle name
                        if (middleName.Length > 0)
                        {
                            middleName = middleName.Trim();

                            if (middleName.Length == 0)
                            {
                                middleNameEntered = false;
                            }

                            middleNameSave = middleName;
                        }
                        else
                        {
                            // Middle Name Unchanged
                            middleName = middleNameSave;
                        }

                    }
                }

                else
                {
                    Console.Write("Enter The Contact's Middle Name or Press Enter To Leave It Blank: ");
                    middleName = Console.ReadLine().Trim();

                    if (middleName.Length > 0)
                    {
                        middleNameEntered = true;
                    }
                }
                Console.WriteLine("Middle Name Is Now: " + (middleNameEntered ? "'" + middleName + "'." : "blank."));

                if (lastNameEntered)
                {
                    Console.Write("\n The Last Name Is '"
                        + lastName
                        + "'. Would You Like To Change It? (Y Or N, Press Enter for no): ");
                    answer = Console.ReadLine().Trim();

                    if (answer.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                        answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Enter The Contact's Last Name");
                        Console.WriteLine("Press Spacebar Enter To Erase or Press Enter To Leave Unchanged: ");

                        lastName = Console.ReadLine();

                        // Check for the new first name or erased last name
                        if (lastName.Length > 0)
                        {
                            lastName = lastName.Trim();

                            if (lastName.Length == 0)
                            {
                                lastNameEntered = false;
                            }

                            lastNameSave = lastName;
                        }
                        else
                        {
                            // Last Name Unchanged
                            lastName = lastNameSave;
                        }

                    }
                }

                else
                {
                    Console.Write("Enter The Contact's Last Name or Press Enter To Leave It Blank: ");
                    lastName = Console.ReadLine().Trim();

                    if (lastName.Length > 0)
                    {
                        lastNameEntered = true;
                    }
                }

                Console.WriteLine("last Name Is Now: " + (lastNameEntered ? "'" + lastName + "'." : "blank."));

                if (firstNameEntered || lastNameEntered)
                {
                    Console.Write("\nWould You Like To Make Any Changes? (Yes or No -or- press Enter for no): ");
                    answer = Console.ReadLine();
                    answer = answer.Trim();

                    if (answer.Length == 0 ||
                        answer.Equals("n", StringComparison.OrdinalIgnoreCase) ||
                        answer.Equals("no", StringComparison.OrdinalIgnoreCase))
                    {



                        basicContact.FirstName = firstName;
                        basicContact.MiddleName = middleName;
                        basicContact.LastName = lastName;
                        resultSet = _contacts.AddBasicContacts(basicContact);


                        if (resultSet.CriticalError || resultSet.LogicalError)
                        {
                            Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:S}", "Error Type", "ClassName", "Membername", "ErrorNumber");

                            foreach (Trace trace in resultSet.Traces)
                            {

                                Console.WriteLine("{0, -15:S} {1,20:S} {2,20:S} {3,25:D}",
                                    trace.ErrorType.ToString(),
                                    trace.ClassName,
                                    trace.MemberName,
                                    trace.ErrorNumber);

                                Console.WriteLine("Error Messages:");
                                foreach (string errorMessage in trace.ErrorMessages)
                                {
                                    Console.WriteLine(errorMessage);
                                }

                                Console.WriteLine();
                            }

                        }

                        else
                        {
                            Console.WriteLine("\nNew Contact With ID: " + basicContact.Id + (firstNameEntered ? " First Name: " + firstName : "") + (middleNameEntered ? ", Middle Name: " + middleName + "," : "") + (lastNameEntered ? " And Last Name: " + lastName : "") + " Added.");
                        }
                        finished = true;
                    }
                }
                else
                {
                    Console.WriteLine("\n Either First or Last Name Must Be Entered");

                    Console.Write("\nWould You Like To Cancel Adding A Contact? (Yes or No -or- press Enter for no): ");
                    answer = Console.ReadLine();
                    answer = answer.Trim();

                    if (answer.Equals("y", StringComparison.OrdinalIgnoreCase) ||
                        answer.Equals("yes", StringComparison.OrdinalIgnoreCase))
                    {
                        finished = true;
                    }
                }
            }
            while (!finished);

            Console.WriteLine();

            return resultSet;
        }

        private int AddEmailAddresses(int contactId)
        {
            EmailAddressModel emailAddressModel = new EmailAddressModel();
            string emailAddress = "";
            bool finished = false;
            bool emailAddressEntered = false;


            while (!finished) 
            {
                Console.Write("Enter an email address -or- enter 'Enter' to exit");
                emailAddress = Console.ReadLine();
                if (emailAddress.Length > 0)
                {

                    emailAddressEntered = true;
                    emailAddressModel.EmailAddress = emailAddress;
                    if (Tools.ValidateEmailAddressCharacters(emailAddress))
                    {
                        if(Tools.ValidateEmailAddressCharacters(emailAddress))
                        {
                            emailAddressModel.EmailAddress = emailAddress;
                            ResultSet<int> resultSet = _contacts.AddEmailAddress(emailAddressModel);
                        }
                        else
                        {
                            Console.Write("invalid email address format");
                            Console.Write(Tools.InvalidEmailFormat);

                        }
                    }
                    else
                    {
                        Console.Write("invalid email address character");
                        Console.Write(Tools.InvalidEmailCharacter);

                    }
                }
                else
                {
                    finished = true;
                }

            }
            return 0;
        }


        private int AddPhoneNumber(int contactId)
        {
            return 0;
        }
    }
}


    


