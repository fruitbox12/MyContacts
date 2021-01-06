using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Linq;
using SQLContactsLibrary.Models;
using DataAccessLibrary;
using Settings;

using Microsoft.Extensions.Configuration;

namespace SQLContactsLibrary
{
    public class SQLContacts : IContacts
    {
        private readonly IAppSettings _appSettings;
        private readonly IDataAccess _dataAccess;

        public SQLContacts(IAppSettings appSettings ,IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _appSettings = appSettings;
        }

        // Getters
        public ResultSet<int> GetNumberOfContacts()
        {
            string sqlStatement = "SELECT COUNT (*) FROM dbo.contacts;";
            return _dataAccess.Count(sqlStatement);
        }

        public ResultSet<BasicContactModel> GetBasicContactById(int contactId)
        {
            // Use @ paramater instead of concating string valiues in SQL statements
            /// TheParameters prevent sql injection hacks https://www.w3schools.com/sql/sql_injection.asp

            //   basiccontactmodel is not found, a new model is created
            string sqlStatement = @"SELECT id AS Id, first_name AS FirstName, middle_name AS MiddleName, last_name AS LastName
                                    FROM dbo.contacts WHERE id = @Id;";
            return _dataAccess.Read<BasicContactModel, dynamic>(sqlStatement, new { Id = contactId });


        }

        public ResultSet<List<BasicContactModel>> GetBasicContactsByName()
        {
            // Using @ sign instead of string concation 
            string sqlStatement = @"SELECT id AS Id, first_name AS FirstName, middle_name AS MiddleName, last_name AS LastName
                                    FROM dbo.contacts ORDER BY last_name, first_name ASC;";
            return _dataAccess.ReadList<BasicContactModel, dynamic>(sqlStatement, new { });
        }


        public ResultSet<List<BasicContactModel>> GetBasicContactsById()
        {
            //  @ sign opposed to string concation
            string sqlStatement = @"SELECT id AS Id, first_name AS FirstName, middle_name AS MiddleName, last_name AS LastName
                                    FROM dbo.contacts;";
            return _dataAccess.ReadList<BasicContactModel, dynamic>(sqlStatement, new { });
        }




        ///
        /// Composite getter methods
        /// 
        /// IN CS, function composition is called an act or mechanism to combine simple functions(methods)
        /// to create 

        public ResultSet<List<ContactModel>> GetContactsByName()
        {
            ResultSet<List<ContactModel>> resultSet = new ResultSet<List<ContactModel>>();

            ResultSet<List<BasicContactModel>> basicContactResultSet = GetBasicContactsByName();
            resultSet.Merge(basicContactResultSet);

            if (basicContactResultSet.Result.Count > 0)
            {
                foreach (var basicContactModel in basicContactResultSet.Result)
                {
                    ContactModel contactModel = new ContactModel();
                    contactModel.BasicContactModel = basicContactModel;
                    // Get the Contact's Email Addresses
                    ResultSet<List<EmailAddressModel>> emailResultSet = GetEmailAddresses(basicContactModel.Id);
                    contactModel.EmailAddress = emailResultSet.Result;
                    resultSet.Merge(emailResultSet);
                    // Get the Contact's Phone Numbers
                    ResultSet<List<PhoneNumberModel>> phoneResultSet = GetPhoneNumbers(basicContactModel.Id);
                    contactModel.PhoneNumber = phoneResultSet.Result;

                    resultSet.Merge(phoneResultSet);
                    resultSet.Result.Add(contactModel);

                }
            }


            return resultSet;
        }

        //simple getter methods
        public ResultSet<List<ContactModel>> GetContactsById()
        {
            ResultSet<List<ContactModel>> resultSet = new ResultSet<List<ContactModel>>();
            ResultSet<List<BasicContactModel>> basicContactResultSet = GetBasicContactsById();
            resultSet.Merge(basicContactResultSet);

            foreach (var basicContactModel in basicContactResultSet.Result)
            {
                ContactModel contactModel = new ContactModel();
                contactModel.BasicContactModel = basicContactModel;
                ResultSet<List<EmailAddressModel>> emailResultSet = GetEmailAddresses(basicContactModel.Id);
                contactModel.EmailAddress = emailResultSet.Result;
                ResultSet<List<PhoneNumberModel>> phoneResultSet = GetPhoneNumbers(basicContactModel.Id);
                contactModel.PhoneNumber = phoneResultSet.Result;


                resultSet.Merge(emailResultSet);
                resultSet.Merge(phoneResultSet);
                resultSet.Result.Add(contactModel);
            }
            return resultSet;
        }


        //Use refleciton to pass a generic instance and get its type, properties and methods at excecution

        public ResultSet<List<BasicContactModel>> SearchBasicContactsByName(string firstName, string lastName)
        {
            // USe @ paramater instead of concating string valiues in SQL statements
            // Parameters prevent sql injection hacks https://www.w3schools.com/sql/sql_injection.asp

            // if no id for basiccontantmodel is not found, null will be returned
            string sqlStatement;
            sqlStatement = @"SELECT id AS Id, first_name AS FirstName, last_name AS LastName
                                    FROM dbo.contacts 
                                    WHERE first_name LIKE @firstName AND last_name LIKE @lastName;";

            return _dataAccess.ReadList<BasicContactModel, dynamic>(sqlStatement, new { FirstName = "%" + firstName + "%", LastName = "%" + lastName + "%" });
        }

        public ResultSet<List<EmailAddressModel>> SearchEmail(string emailAddress)
        {

            //the correct query stored in my sqlscripts folder
            // i must finish implementing
            string sqlStatement = @"SELECT id AS Id, email_address AS EmailAddress
                                    FROM dbo.email_addresses                           
                                    WHERE email_address LIKE @emailAddress;";
            return _dataAccess.ReadList<EmailAddressModel, dynamic>(sqlStatement, new { EmailAddress = "%" + emailAddress + "%" });
        }

        public ResultSet<BasicContactModel> GetBasicContactByEmail(int emailAddressId)
        {
            // Must use column name aliases equivalent to the contanct model
            string sqlStatement = @"SELECT c.id as Id, c.first_name as FirstName, c.middle_name as MiddleName, c.last_name as LastName
                                    From dbo.contacts as c
                                    Inner Join dbo.contacts_email_addresses as ce
                                    ON ce.contact_id = c.id
                                    WHERE ce.email_id = @Id;";


            ResultSet<BasicContactModel> resultSet = _dataAccess.Read<BasicContactModel, dynamic>(sqlStatement, new { Id = emailAddressId });

            // IF not found indicates a dnagling email address aka one not associated with contact bc FK relatiosnhip with contacts email table
            // Not sql error, not critical then
            // Flag as a critical error
            if (resultSet.CriticalError == false && resultSet.LogicalError == false && resultSet.Result.Id == 0)
            {
                // Do not flag a ciritical error yetm wait until we try to fix it
                resultSet.ErrorMessage = "Email Address with Id " +
                                            +emailAddressId
                                            + " in the Email Address table is not associated with any contact.";

                Trace trace = new Trace();
                trace.ErrorType = Trace.ErrorTypes.Critical;
                trace.ClassName = "GetBasicContactByEmail";
                trace.MemberName = "SqlContacts";
                trace.ErrorMessages.Add("Email Address with Id "
                                        + emailAddressId
                                        + " in the Email Addresses table is not associated with any Contact.");
                trace.ErrorMessages.Add("Email Address with Id "
                                        + emailAddressId
                                        + " not found on the Contacts Email Addresses FK table.");
                resultSet.AddTrace(trace);


                sqlStatement = @"DELETE FROM dbo.email_addresses where id = @EmailAddressId;";

                ResultSet<int> emailResultSet = _dataAccess.Delete<int, dynamic>(sqlStatement, new { EmailAddressId = emailAddressId });
                resultSet.Merge(emailResultSet);

                trace = new Trace();
                trace.ClassName = "GetBasicContactByEmail()";
                trace.MemberName = "SqlContacts";

                // Do not set the basiccontactmodel  result id to zero, it will confuse view. Not basiccontactmodel id, what delete returns
                // same thing, less readable if (emailResultSet.CriticalError == false && emailResultSet.LogicalError == true && emailResultSet.Result == 0)
                //if ((emailResultSet.CriticalError == true || emailResultSet.LogicalError == true) && emailResultSet.Result == 0)            

                if (emailResultSet.Result == 1)
                {
                    // successful delete
                    resultSet.ErrorMessage = "Email Address with Id "
                                        + emailAddressId
                                        + " in the Email Addresses table is not associated with any Contact, successfully deleted";
                    trace.ErrorMessages.Add("Email Address with Id "
                                        + emailAddressId
                                        + " in the Email Addresses table is not associated with any Contact, successfully deleted");
                    resultSet.AddTrace(trace);
                }
                else
                {
                    // unseccessful delete
                    resultSet.CriticalError = true;
                    resultSet.ErrorMessage = "Email Address with Id "
                                        + emailAddressId
                                        + " in the Email Addresses table is not associated with any Contact, unable to be deleted";
                    trace.ErrorType = Trace.ErrorTypes.Critical;
                    trace.addErrorMessage("Email Address with Id "
                                        + emailAddressId
                                        + " in the Email Addresses table is not associated with any Contact, unable to be deleted");
                    resultSet.AddTrace(trace);

                }
            }


            return resultSet;

        }


        public ResultSet<List<EmailAddressModel>> GetEmailAddresses(int contactId)
        {
            // retrieve all email addresses for this contact
            string sqlStatement = @"SELECT e.id AS Id, e.email_address as EmailAddress
                                FROM dbo.email_addresses AS e
                                INNER JOIN dbo.contacts_email_addresses as ce
                                ON e.id = ce.email_id
                                WHERE ce.contact_id = @Id;";
            return _dataAccess.ReadList<EmailAddressModel, dynamic>(sqlStatement, new { Id = contactId });
        }

        public ResultSet<List<PhoneNumberModel>> GetPhoneNumbers(int contactId)
        {
            // retrieve all email addresses for this contact
            string sqlStatement = @"SELECT p.id AS Id, p.phone_number as PhoneNumber   
                                FROM dbo.phone_numbers AS p
                                INNER JOIN dbo.contacts_phone_numbers as cp
                                ON p.id = cp.phone_number_id
                                WHERE cp.contact_id = @Id;";
            return _dataAccess.ReadList<PhoneNumberModel, dynamic>(sqlStatement, new { Id = contactId });
        }

        public ResultSet<List<PhoneNumberModel>> SearchPhoneNumbers(string phoneNumber)
        {
            // 
            // Parameters prevent sql injection hacks https://www.w3schools.com/sql/sql_injection.asp


            string sqlStatement = @"SELECT id AS Id, phone_number AS PhoneNumber
                                FROM dbo.phone_numbers                            
                                WHERE phone_number LIKE @phoneNumber;";

            return _dataAccess.ReadList<PhoneNumberModel, dynamic>(sqlStatement, new { PhoneNumber = "%" + phoneNumber + "%" });
        }
        public ResultSet<BasicContactModel> UpdateBasicContacts(BasicContactModel basicContactModel)
        {
            string sqlStatement = @"UPDATE dbo.contacts
                                    SET first_name = @FirstName, middle_name = @MiddleName, last_name = @LastName
                                    WHERE id = @Id;";

            return _dataAccess.Read<BasicContactModel, dynamic>(sqlStatement, new { Id = basicContactModel.Id, FirstName = basicContactModel.FirstName, MiddleName = basicContactModel.MiddleName, LastName = basicContactModel.LastName });
        }
        public bool ValidateContactId(int contactId)
        {
            bool IdFound = false;
            string sqlStatement = @"SELECT id FROM dbo.contacts WHERE id = @Id;";

            ResultSet<int> id = _dataAccess.Read<int, dynamic>(sqlStatement, new { Id = contactId });

            if (id.Result > 0)
            {
                IdFound = true;
            }

            return IdFound;
        }
        public ResultSet<BasicContactModel> GetBasicContactByPhoneNumber(int phoneNumberId)
        {
            string sqlStatement = @"SELECT c.id as Id, c.first_name as FirstName, c.middle_name as MiddleName, c.last_name as LastName
                                    From dbo.contacts as c
                                    Inner Join dbo.contacts_phone_numbers as cp
                                    ON cp.contact_id = c.id
                                    WHERE cp.phone_number_id = @Id;";

            ResultSet<BasicContactModel> resultSet = _dataAccess.Read<BasicContactModel, dynamic>(sqlStatement, new { Id = phoneNumberId });

            // IF not found indicates a dnagling Phone Number aka one not associated with contact bc FK relatiosnhip with contacts Phone Number table
            // Not sql error, not critical then
            // Flag as a critical error
            if (resultSet.CriticalError == false && resultSet.LogicalError == false && resultSet.Result.Id == 0)
            {
                // Do not flag a ciritical error yetm wait until we try to fix it
                resultSet.ErrorMessage = "Phone Number with Id " +
                                            +phoneNumberId
                                            + " in the Phone Number table is not associated with any contact.";

                Trace trace = new Trace();
                trace.ErrorType = Trace.ErrorTypes.Critical;
                trace.ClassName = "GetBasicContactByPhoneNumber()";
                trace.MemberName = "SqlContacts";
                trace.ErrorMessages.Add("Phone Number with Id "
                                        + phoneNumberId
                                        + " in the Phone Number table is not associated with any Contact.");
                trace.ErrorMessages.Add("Phone Number with Id "
                                        + phoneNumberId
                                        + " not found on the Contacts Phone Number FK table.");
                resultSet.AddTrace(trace);


                sqlStatement = @"DELETE FROM dbo.phone_numbers where id = @PhoneNumberId;";

                ResultSet<int> phoneNumberResultSet = _dataAccess.Delete<int, dynamic>(sqlStatement, new { PhoneNumberId = phoneNumberId });
                resultSet.Merge(phoneNumberResultSet);

                trace = new Trace();
                trace.ClassName = "GetBasicContactByPhoneNumber()";
                trace.MemberName = "SqlContacts";

                // Do not set the basiccontactmodel  result id to zero, it will confuse view. Not basiccontactmodel id, what delete returns
                // same thing, less readable if (phoneNumberResultSet.CriticalError == false && phoneNumberResultSet.LogicalError == true && phoneNumberResultSet.Result == 0)
                //if ((phoneNumberResultSet.CriticalError == true || phoneNumberResultSet.LogicalError == true) && phoneNumberResultSet.Result == 0)            

                if (phoneNumberResultSet.Result == 1)
                {
                    // successful delete

                    resultSet.ErrorMessage = "Phone Number with Id "
                                        + phoneNumberId
                                        + " in the Phone Number table is not associated with any Contact, successfully deleted";
                    trace.ErrorMessages.Add("Phone Number with Id "
                                        + phoneNumberId
                                        + " in the Phone Number table is not associated with any Contact, successfully deleted");
                    resultSet.AddTrace(trace);
                }
                else
                {
                    // unseccessful delete
                    resultSet.CriticalError = true;
                    resultSet.ErrorMessage = "Phone Number with Id "
                                        + phoneNumberId
                                        + " in the Phone Number table is not associated with any Contact, unable to be deleted";
                    trace.ErrorType = Trace.ErrorTypes.Critical;
                    trace.addErrorMessage("Phone Number with Id "
                                        + phoneNumberId
                                        + " in the Phone Number table is not associated with any Contact, unable to be deleted");
                    resultSet.AddTrace(trace);

                }
            }


            return resultSet;

        }


        public ResultSet<BasicContactModel> AddBasicContacts(BasicContactModel basicContactModel)
        {
            ResultSet<BasicContactModel> resultSet = new ResultSet<BasicContactModel>(basicContactModel);

            //Valid Names are not all blank
            if (basicContactModel.FirstName.Length > 0 || basicContactModel.LastName.Length > 0)
            {

            string sqlStatement = @"INSERT INTO dbo.contacts (first_name, middle_name, last_name)
                                    VALUES (@FirstName,@MiddleName, @LastName);";

            ResultSet<int> createResultSet = _dataAccess.Create<BasicContactModel, dynamic>(sqlStatement,
                                                                                        new
                                                                                        {
                                                                                            FirstName = basicContactModel.FirstName,
                                                                                            MiddleName = basicContactModel.MiddleName,
                                                                                            LastName = basicContactModel.LastName
                                                                                       
                                                                                        });

            resultSet.Result.Id = createResultSet.Result;
            resultSet.Merge(createResultSet);

            }
            else
            {
                resultSet.LogicalError = true;
                Trace trace = new Trace();
                trace.ErrorType = Trace.ErrorTypes.Logical;
                trace.ClassName = "SQLContacts";
                trace.MemberName = "AddBasicContacts()";
                trace.ErrorMessages.Add("Error Message: One of 'First Name' or 'Last Name' must be entered");
                resultSet.Traces.Add(trace);
            }

            return resultSet;

        }
        public ResultSet<int> AddEmailAddress(int contactId, EmailAddressModel emailAddressModel)
        {
            ResultSet<int> resultSet = new ResultSet<int>();

            //Valid Names are not all blank

            string sqlStatement = @"INSERT INTO dbo.email_addresses (email_address)
                                    VALUES (@EmailAddress);";


            ResultSet<int> emailResultSet = _dataAccess.Create<EmailAddressModel, dynamic>(sqlStatement,
                                                    new{EmailAddress = emailAddressModel.EmailAddress});

            resultSet.Merge(emailResultSet);
            resultSet.Result = emailResultSet.Result;

            //for readability check for false you can use !addResultSet.CriticalError
            if (emailResultSet.CriticalError == false && emailResultSet.LogicalError == false)
            {
                ContactEmailModel contactEmailModel = new ContactEmailModel();
                contactEmailModel.ContactId = contactId;
                contactEmailModel.EmailId = emailResultSet.Result;

                ResultSet<int> contactEmailResultSet = AddContactEmailAddress(contactEmailModel);

                resultSet.Merge(contactEmailResultSet);
                resultSet.Result = contactEmailResultSet.Result;
            }
            else if (resultSet.ErrorNumber == 2601)
            {
                resultSet.ErrorMessage = "'" + "\nEmail Address" + emailAddressModel.EmailAddress + " is in use";
            }
            return resultSet;
            /* else
             {


                  sqlStatement = @"DELETE FROM dbo.email_addresses 
                                     WHERE contact_id = @ContactId and email_id = @EmailId;";

                 ResultSet<int> deleteResultSet = _dataAccess.Delete<EmailAddressModel, dynamic>(sqlStatement,
                                                   new { EmailAddress = emailAddressModel.EmailAddress });
                 resultSet.Merge(deleteResultSet);
                 resultSet.Result = deleteResultSet.Result;
             }
            */

        }

        public ResultSet<int> AddPhoneNumber(int contactId, PhoneNumberModel phoneNumberModel)
        {
            ResultSet<int> resultSet = new ResultSet<int>();

            //Valid Names are not all blank


            string sqlStatement = @"INSERT INTO dbo.phone_numbers (phone_number)
                                    VALUES (@PhoneNumber);";


            ResultSet<int> phoneNumberResultSet = _dataAccess.Create<PhoneNumberModel, dynamic>(sqlStatement,
                                                    new { PhoneNumber = phoneNumberModel.PhoneNumber });

            resultSet.Merge(phoneNumberResultSet);
            resultSet.Result = phoneNumberResultSet.Result;

       

            //for readability check for false you can use !addResultSet.CriticalError
            if (phoneNumberResultSet.CriticalError == false && phoneNumberResultSet.LogicalError == false)
            {
                ContactPhoneNumberModel contactPhoneNumberModel = new ContactPhoneNumberModel();
                contactPhoneNumberModel.ContactId = contactId;
                contactPhoneNumberModel.PhoneNumberId = phoneNumberResultSet.Result;

                ResultSet<int> contactPhoneNumberResultSet = AddContactPhoneNumber(contactPhoneNumberModel);

                resultSet.Merge(contactPhoneNumberResultSet);
                resultSet.Result = contactPhoneNumberResultSet.Result;
            }
            else if (resultSet.ErrorNumber == 2601)
            {
                resultSet.ErrorMessage = "'" + "\nPhone Number" + phoneNumberModel.PhoneNumber + " is in use";
            }
            return resultSet;

        }
        public ResultSet<int> AddContactEmailAddress(ContactEmailModel contactEmailModel)
        {

            string sqlStatement = @"INSERT INTO dbo.contacts_email_addresses (contact_id, email_id)
                                    VALUES (@ContactId, @EmailId);";


            ResultSet<int> resultSet = _dataAccess.Create<EmailAddressModel, dynamic>(sqlStatement, new { ContactId = contactEmailModel.ContactId,
                                                                                                            EmailId = contactEmailModel.EmailId
                                                                                                           });
            return resultSet;

        }


        public ResultSet<int> AddContactPhoneNumber(ContactPhoneNumberModel contactPhoneNumberModel)
        {

            string sqlStatement = @"INSERT INTO dbo.contacts_phone_numbers (contact_id, phone_number_id)
                                    VALUES (@ContactId, @PhoneNumberId);";


            ResultSet<int> resultSet = _dataAccess.Create<PhoneNumberModel, dynamic>(sqlStatement, new
            {
                ContactId = contactPhoneNumberModel.ContactId,
                PhoneNumberId = contactPhoneNumberModel.PhoneNumberId
            });
            return resultSet;

        }



    }
}


