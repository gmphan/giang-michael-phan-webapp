using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib.ViewModels
{
    public class ContactMeDTO
    {
        [DisplayName("First Name")]
        public string? FirstName { get; set; }
        [DisplayName("Last Name")]
        public string? LastName { get; set; }
        public string? Email { get; set; }
        [DisplayName("Phone Number")]
        public string? PhoneNum { get; set; }
        public string? Message { get; set; }   
        public DateTime CreatedDate { get; set; }
    }
}