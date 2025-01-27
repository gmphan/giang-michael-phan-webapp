using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Gmphan.ModelLib
{
    public class ProfilePicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Add this for binary storage
        public byte[]? PictureData { get; set; }

        // true for profile picture, false for all other pictures
        public bool IsProfilePicture { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}