using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

        public List<ContactModel> GetContacts()
        {
            List<ContactModel> contactModels = new List<ContactModel>();

            List<BasicContactModel> rows = new List<BasicContactModel>();

            if (rows.Count > 0)
            {
                foreach (var row in rows)
                {
                    contactModels.Add(GetContactById(row.id));
                }
            }

            return contactModels;
        }
        /*
         * integrated Solutions
         */

        public ContactModel GetContactById(int id)
        {
            ContactModel contactModel = new ContactModel();

            string sqlStatement = @"SELECT id AS Id, first_name AS FirstName, middle_name AS MiddleName, last_name AS LastName
                                    FROM dbo.contacts WHERE id = @Id;";
            contactModel.BasicContactModel = _dataAccess.Read<BasicContactModel, dynamic>(sqlStatement, new { Id = id }).FirstOrDefault();

            if (contactModel.BasicContactModel != null)
            {
                //Retrieve all Email addresses for this contact
                sqlStatement = @"SELECT e.id AS Id, e.email_address AS EmailAddress
                                 FROM dbo.email_addresses AS e
                                 INNER JOIN dbo.contact_email_addresses as ce
                                 ON e.id = ce.email_id
                                 WHERE ce.contact_id = @Id";

                contactModel.EmailAddress = _dataAccess.Read<EmailAddressModel, dynamic>(sqlStatement, new { Id = id });

                sqlStatement = @"SELECT p.id AS Id, p.phone_number AS PhoneNumber
                                 FROM dbo.phone_numbers AS p
                                 INNER JOIN dbo.contact_phone as cp
                                 ON p.id = cp.phone_number_id
                                 WHERE ce.contact_id = @Id";

                contactModel.PhoneNumber = _dataAccess.Read<PhoneNumberModel, dynamic>(sqlStatement, new { Id = id });
                //Retrieve all phone numbers

            }
            else
            {
                contactModel.BasicContactModel = new BasicContactModel();
            }
            return contactModel;
        }
        // Getters

        public int GetNumberOfContacts()
        {
            string sqlStatement = "SELECT COUNT (*) FROM dbo.contacts;";
            return _dataAccess.Count(sqlStatement);
        }

        public List<BasicContactModel> GetBasicContacts()
        {

            //dyunamic sqlstatement (vs Stored Procedure)
            string sqlStatement = @"SELECT id AS Id, first_name AS FirstName, middle_name AS MiddleName, last_name AS LastName
                                    FROM dbo.contacts ORDER BY last_name, first_name ASC;";
            //Using @ prefix instead of string concatenation easier to write long sdtring values
            //DO not assign a value to a variable just to return it
            return _dataAccess.Read<BasicContactModel, dynamic>(sqlStatement, new { });
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
