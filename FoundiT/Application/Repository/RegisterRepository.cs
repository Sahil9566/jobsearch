using Application.Repository.IRepository;
using Domain.DTOs;
using Domain.Models;
using Infastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly UserManager<Register> _userManager;
        private readonly Applicationdbcontext _context;
        public RegisterRepository(Applicationdbcontext context, UserManager<Register> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<(bool, Register)> Register(RegisterDTO register)
        {

            var existingUser = await _context.Registers.FirstOrDefaultAsync(u => u.Email == register.Email || u.PhoneNumber == register.PhoneNumber);
            if (existingUser is not null)
            {
                return (false, null);
            }
            else
            {
                var user = new Register()
                {
                    Email = register.Email,
                    PhoneNumber = register.PhoneNumber,
                    Name = register.Name,
                    UserName = register.Email,
                    ResumeUrl = ""
                };
                var isSuccess = await _userManager.CreateAsync(user, register.Password);
                return (true, user);
                //if (isSuccess.Succeeded)
                //{

                //}
                //else
                //{

                //}

            }
        }
    }
}
