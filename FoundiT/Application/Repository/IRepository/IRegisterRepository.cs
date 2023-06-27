using Domain.DTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository.IRepository
{
    public interface IRegisterRepository
    {
        Task<(bool, Register)> Register(RegisterDTO register);
    }
}
