using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace ASP.NET_Console_Project
{
    class SQLContacts
    {
        private readonly SQLDataAccess _dataAccess;

        public SQLContacts()
        {

            _dataAccess = new SQLDataAccess();

        }

        // Getters
        public int GetNumberOfContacts()
        {
            string sqlStatement = "SELECT COUNT (*) FROM dbo.contacts;";
            return _dataAccess.Count(sqlStatement);
        }

        public ContactModel GetContactById(int id)
        {
            // USe @ paramater instead of concating string valiues in SQL statements
            // Parameters prevent sql injection hacks https://www.w3schools.com/sql/sql_injection.asp
            ContactModel contactModel = new ContactModel();

            string sqlStatement = @"SELECT id AS Id, first_name AS FirstName, middle_name AS MiddleName, last_name AS LastName
                                    FROM dbo.contacts WHERE id = @Id;";

            // if no id for basiccontantmodel is not found, null will be returned
            contactModel.BasicContactModel = _dataAccess.Read<BasicContactModel, dynamic>(sqlStatement, new { Id = id }).FirstOrDefault();

            if (contactModel.BasicContactModel != null)
            {
                // retrieve all email addresses for this contact
                sqlStatement = @"SELECT e.id AS Id, e.email_address as EmailAddress
                                FROM dbo.email_addresses AS e
                                INNER JOIN dbo.contact_email_addresses as ce
                                ON e.id = ce.email_id
                                WHERE ce.contact_id = @Id;";
                contactModel.EmailAddress = _dataAccess.Read<EmailAddressModel, dynamic>(sqlStatement, new { Id = id });

                // retrieve all phone numbers for this contact
                sqlStatement = @"SELECT p.id AS Id, p.phone_number as PhoneNumber
                                FROM dbo.phone_numbers AS p
                                INNER JOIN dbo.contact_phone_numbers as cp
                                ON p.id = cp.phone_number_id
                                WHERE cp.contact_id = @Id;";
                contactModel.PhoneNumber = _dataAccess.Read<PhoneNumberModel, dynamic>(sqlStatement, new { Id = id });

            }
            else
            {
                contactModel.BasicContactModel = new BasicContactModel();
            }

            return contactModel;
        }
        public List<ContactModel> GetContactsById()
        {


            List<ContactModel> contactModels = new List<ContactModel>();
            string sqlStatement = @"SELECT id AS Id, first_name AS FirstName, middle_name AS MiddleName, last_name AS LastName
                            FROM dbo.contacts;";
            List<BasicContactModel> basicContactModels = _dataAccess.Read<BasicContactModel, dynamic>(sqlStatement, new { });

            foreach (var basicContactModel in basicContactModels)
            {
                ContactModel contactModel = new ContactModel();
                contactModel.BasicContactModel = basicContactModel;
                sqlStatement = @"SELECT e.id AS Id, e.email_address as EmailAddress
                                FROM dbo.email_addresses AS e
                                INNER JOIN dbo.contact_email_addresses as ce
                                ON e.id = ce.email_id
                                WHERE ce.contact_id = @Id;";
                contactModel.EmailAddress = _dataAccess.Read<EmailAddressModel, dynamic>(sqlStatement, new { Id = basicContactModel.Id });

                sqlStatement = @"SELECT p.id AS Id, p.phone_number as PhoneNumber
                                FROM dbo.phone_numbers AS p
                                INNER JOIN dbo.contact_phone_numbers as cp
                                ON p.id = cp.phone_number_id
                                WHERE cp.contact_id = @Id;";
                contactModel.PhoneNumber = _dataAccess.Read<PhoneNumberModel, dynamic>(sqlStatement, new { Id = basicContactModel.Id });

                contactModels.Add(contactModel);
            }

            return contactModels;
        }
        public List<BasicContactModel> GetBasicContacts()
        {
            // Using @ sign instead of string concation makes it easier to add values
            string sqlStatement = @"SELECT id AS Id, first_name AS FirstName, middle_name AS MiddleName, last_name AS LastName
                                    FROM dbo.contacts ORDER BY last_name, first_name ASC;";
            return _dataAccess.Read<BasicContactModel, dynamic>(sqlStatement, new { });
        }



        // Integrated Solutions
        public List<ContactModel> GetContacts()
        {
            List<ContactModel> contactModels = new List<ContactModel>();

            List<BasicContactModel> rows = GetBasicContacts();


            if (rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    // Use our own helper method
                    contactModels.Add(GetContactById(row.Id));
                }
            }

            return contactModels;
        }



        private string GetConnectionString(string connectionString = "Default")
        {
            string connection = "";

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

            var config = builder.Build();

            connection = config.GetConnectionString(connectionString);
            return connection;
        }

    }
}
