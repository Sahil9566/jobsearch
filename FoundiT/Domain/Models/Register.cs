using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Register : IdentityUser
    {
        //[NotMapped]
       // public IFormFile ImageFile { get; set; }
       // public string  ResumeUrl { get; set; }

        public string Name { get; set; }
       
        public Gender Gender { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string EducationaldetailsId { get; set; }
        public EducationDetails EducationDetails { get; set; }




    }  
}
