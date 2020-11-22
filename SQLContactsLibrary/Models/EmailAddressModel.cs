using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace SQLContactsLibrary.Models
{
   public class EmailAddressModel
    {

        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = "";
    }
}
