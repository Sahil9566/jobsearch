using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }

    }
}
