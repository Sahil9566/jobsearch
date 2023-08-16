using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.DTOs
{
    public class RegisterDTO
    {
       // [NotMapped]
       // public IFormFile ImageFile { get; set; }

        public string Name { get; set; }
        public Gender? Gender { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }

        //EducationDetails
        [NotMapped]
        public string HighestQualification { get; set; }
        [NotMapped]
        public string SelectYourField { get; set; }
        [NotMapped]
        public string University_Insitute { get; set; }
        [NotMapped]
        public string YearOfGraduation { get; set; }
        [NotMapped]
        public Education EducationType { get; set; }


        //ProfessionalDetails
        [NotMapped]

        public Experience Experience { get; set; }
        [NotMapped]
        public string CurrentLocation { get; set; }


        //JobPrefrence

        [NotMapped]
        public string KeySkills { get; set; }
        [NotMapped]
        public string Industry { get; set; }
        [NotMapped]
        public string Department { get; set; }
        [NotMapped]
        public string PrefrredRole { get; set; }
        [NotMapped]

        public string PrefrredLocation { get; set; }

    }
}
