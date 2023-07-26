using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class VerifyOTPVM
    {
        public string OTP { get; set; }
        public string PhoneNumber { get; set; }
    }
}
