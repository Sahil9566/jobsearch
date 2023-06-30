using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository.IRepository
{
    public interface ISmsRepository
    {
        Task<bool> SendSMSPinWithBasicAuth(string PhoneNumber);
        Task<bool> VerifyOTP(string OTP);
    }  
}


