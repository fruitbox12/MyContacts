using System;
using System.Collections.Generic;
using System.Text;

namespace SQLContactsLibrary.Models
{

   public class ContactModel
    {
        public BasicContactModel BasicContactModel { get; set; }
        public List<EmailAddressModel>  EmailAddress { get; set; }
        public List<PhoneNumberModel>  PhoneNumber {get; set;}
    }
}
