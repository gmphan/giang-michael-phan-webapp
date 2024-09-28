using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib
{
    public class ResumeHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        // MaxLength will be implemented directly into the view, so
        //in FirstName of the view, I can't type over 20 characters.
        [MaxLength(20)]
        [DisplayName("First Name")]
        public string? FirstName { get; set; }
         [MaxLength(20)]
        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }
        [Required]
        [DisplayName("Last Name")]
        public string? LastName { get; set; }
        [Required]
        public string? Headline { get; set; }
        [DisplayName("Phone Number")]
        [MaxLength(12)]
        public string? PhoneNum { get; set; }
        [Required]
        [MaxLength(50)]
        [DisplayName("Your Email")]
        public string? Email { get; set; }
        [Required]
        public string? Country { get; set; }
        [Required]
        [DisplayName("Street Address")]
        public string? StreetAddress { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        [DisplayName("Zip Code")]
        public string? ZipCode { get; set; }
        [DisplayName("Linked In")]
        public string? LinkedIn { get; set; }
        [DisplayName("Git Hub")]
        public string? GitHub { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}