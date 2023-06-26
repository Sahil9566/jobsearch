using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class Register:IdentityUser
    {
        public int Id { get; set; }
        [NotMapped]
        public IFormFile UploadResume { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string EmailId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int MobileNumber { get; set; }
        [Required]
        public bool Gender { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool MobileNumberConfirmed { get; set;}

    }
}
