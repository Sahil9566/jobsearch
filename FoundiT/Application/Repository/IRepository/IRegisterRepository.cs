using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository.IRepository
{
    // register irepository
    public interface IRegisterRepository
    {
        Task<(bool, Register)> Register(RegisterDTO register);
       
    }
}
