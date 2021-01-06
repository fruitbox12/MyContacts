using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SQLContactsLibrary.Models
{
   public class EmailAddressModel
    {

        public int Id { get; set; }
 
        public string EmailAddress { get; set; } = "";
    }
}
