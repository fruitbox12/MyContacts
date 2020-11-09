using DataAccessLibrary;
using SQLContactsLibrary.Models;
using System.Collections.Generic;

namespace SQLContactsLibrary
{
    public interface IContacts
    {
        ResultSet<BasicContactModel> GetBasicContactByEmail(int emailAddressId);
        ResultSet<BasicContactModel> GetBasicContactById(int contactId);
        ResultSet<BasicContactModel> AddBasicContacts(BasicContactModel basicContactModel);

        ResultSet<BasicContactModel> GetBasicContactByPhoneNumber(int phoneNumberId);
        ResultSet<List<BasicContactModel>> GetBasicContactsById();
        ResultSet<List<BasicContactModel>> GetBasicContactsByName();
        ResultSet<List<ContactModel>> GetContactsById();
        ResultSet<List<ContactModel>> GetContactsByName();
        ResultSet<List<EmailAddressModel>> GetEmailAddresses(int contactId);
        ResultSet<int> GetNumberOfContacts();
        ResultSet<List<PhoneNumberModel>> GetPhoneNumbers(int contactId);
        ResultSet<List<BasicContactModel>> SearchBasicContactsByName(string firstName, string lastName);
        ResultSet<List<EmailAddressModel>> SearchEmail(string emailAddress);
        ResultSet<List<PhoneNumberModel>> SearchPhoneNumbers(string phoneNumber);
    }
}