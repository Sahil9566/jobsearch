using Application.Repository.IRepository;
using Azure.Storage.Blobs;
using Domain.DTOs;
using Domain.Models;
using Infastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly UserManager<Register> _userManager;
        private readonly Applicationdbcontext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ISmsRepository _smsRepository;



        public RegisterRepository(Applicationdbcontext context, UserManager<Register> userManager, BlobServiceClient blobServiceClient, ISmsRepository smsRepository)
        {
            _context = context;
            _userManager = userManager;
            _blobServiceClient = blobServiceClient;
            _smsRepository = smsRepository;
        }

        public async Task<(bool, Register)> Register(RegisterDTO register)
        {
            var existingUser = await _context.Registers.FirstOrDefaultAsync(u => u.Email == register.Email || u.PhoneNumber == register.PhoneNumber);
            if (existingUser is not null)
            {
                return (false, null);
            }

            var user = new Register()
            {
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                Name = register.Name,
                UserName = register.Email,
                CreatedOn = DateTime.UtcNow,
            };

            var blobContainer = _blobServiceClient.GetBlobContainerClient("resumefiles");
            var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(register.ImageFile.FileName);

            var blobClient = blobContainer.GetBlobClient(imageFileName);

            // Delete the existing blob file if it exists
            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
            }

            await blobClient.UploadAsync(register.ImageFile.OpenReadStream());

            user.ResumeUrl = blobClient.Uri.ToString(); // Save the URL instead of the file name

            var isSuccess = await _userManager.CreateAsync(user, register.Password);

            if (isSuccess.Succeeded)
            {
                var SendOTP = _smsRepository.SendSMSPinWithBasicAuth(register.PhoneNumber);
                return (true, user);
            }
            else
            {
                return (false, null);
            }
        }
            
     

       
    }
}
