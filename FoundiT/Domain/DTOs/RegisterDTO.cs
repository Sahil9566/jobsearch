﻿using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class RegisterDTO
    {
        //public IFormFile? UploadResume { get; set; }
        //public string? ResumeUrl { get; set; }
        public string Name { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}