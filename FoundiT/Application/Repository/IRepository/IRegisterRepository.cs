using Domain.DTOs;
using Domain.Models;
using Infastructure.Migrations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository.IRepository
{
    
    public interface IRegisterRepository  
    {
        ICollection<Register> GetRegister();
        Task<(bool, Register)> Register(RegisterDTO register);
        Task<bool> VerifyEmail(string email);

    }
}
 