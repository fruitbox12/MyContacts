using System;
using System.Collections.Generic;
using System.Text;

namespace SQLContactsLibrary.Models
{
   public class ContactEmailModel
    {
        public int id { get; set; }
        public int ContactId { get; set; }
        public int EmailId { get; set; }
    }
}
