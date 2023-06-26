using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepository.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly ApplicationDbContext _context;
        public RegisterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Register> Create(Register register)
        {
            try
            {
                var existingUser = await _context.registers.FirstOrDefaultAsync(u => u.Name == register.Name);
                if (existingUser != null)
                {
                    throw new Exception("Register with the same name already exists.");
                }

                var existingData = await _context.registers.FirstOrDefaultAsync(u => u.Email == register.Email || u.PhoneNumber == register.PhoneNumber);
                if (existingData != null)
                {
                    throw new Exception("Register with the same email or phone number already exists.");
                }
                if (existingUser != null && existingData != null)
                {
                    _context.registers.Add(register);
                    await _context.SaveChangesAsync();
                    return register;
                }
            }
            catch (Exception ex) { }
            return null;
        }

    }
}
