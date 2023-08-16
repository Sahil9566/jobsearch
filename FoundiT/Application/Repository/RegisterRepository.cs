using Application.Repository.IRepository;
using Azure.Storage.Blobs;
using Domain.DTOs;
using Domain.Models;
using Infastructure.Data;
using Infastructure.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IMailService _mailService;



        public RegisterRepository(Applicationdbcontext context, UserManager<Register> userManager, BlobServiceClient blobServiceClient, ISmsRepository smsRepository, IMailService mailService)
        {
            _context = context;
            _userManager = userManager;
            _blobServiceClient = blobServiceClient;
            _smsRepository = smsRepository;
            _mailService = mailService;
        }

        public ICollection<ProfessionalDetails> GetProfessionalDetails()
        {
            return _context.Professional_Details.ToList();
        }

        public ICollection<Register> GetRegister()
        {
            return _context.Registers.ToList();
        }

        //public async Task<(bool, Register)> Register(RegisterDTO register)
        //{
        //    var existingUser = await _context.Registers.FirstOrDefaultAsync(u => u.Email == register.Email && u.PhoneNumber == register.PhoneNumber);
        //    if (existingUser is not null)
        //    {
        //        return (false, null);
        //    }

        //    var user = new Register()
        //    {
        //        Email = register.Email,
        //        PhoneNumber = register.PhoneNumber,
        //        Name = register.Name,
        //        UserName = register.Email,
        //        CreatedOn = DateTime.Now,
        //    };




        //    var blobContainer = _blobServiceClient.GetBlobContainerClient("resumefiles");
        //    var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(register.ImageFile.FileName);

        //    var blobClient = blobContainer.GetBlobClient(imageFileName);

        //    // Delete the existing blob file if it exists
        //    if (await blobClient.ExistsAsync())
        //    {
        //        await blobClient.DeleteAsync();
        //    }

        //    await blobClient.UploadAsync(register.ImageFile.OpenReadStream());

        //    user.ResumeUrl = blobClient.Uri.ToString(); // Save the URL instead of the file name
        //    var mail = new MailRequest()
        //    {
        //        Body = $"Please verify your account <a href='http://localhost:3000/emailConfirmation/{register.Name}'>Verify Emails</a>",
        //        Subject = "Verify Email Notification",
        //        ToEmail = register.Email
        //    };



        //    await sendemail(mail); 
        //    var isSuccess = await _userManager.CreateAsync(user, register.Password);
        //    if (isSuccess.Succeeded)
        //    {
        //        var professionalDetails = new ProfessionalDetails();
        //        professionalDetails.RegiserID = user.Id;
        //        professionalDetails.Experience = register.Experience;
        //        professionalDetails.CurrentLocation = register.CurrentLocation;
        //        await _context.Professional_Details.AddAsync(professionalDetails);
        //        await _context.SaveChangesAsync();
        //    }

        //    if (isSuccess.Succeeded)
        //    {


        //        //var SendOTP = _smsRepository.SendSMSPinWithBasicAuth(register.PhoneNumber);
        //        return (true, user);
        //    }
        //    else
        //    {
        //        return (false, null);
        //    }
        //}


        [NonAction]
        public async Task sendemail(MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
            }
            catch (Exception ex)
            {
                // Handle exception
            }
        }

        public async Task<bool> VerifyEmail(string email)
        {
            var user = await _context.Registers.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return false;
            }
            user.EmailConfirmed = true;
            user.UpdatedOn = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<(bool, Register, ProfessionalDetails)> Register(RegisterDTO register)
        {
            var existingUser = await _context.Registers.FirstOrDefaultAsync(u => u.Email == register.Email && u.PhoneNumber == register.PhoneNumber);
            if (existingUser is not null)
            {
                return (false, null, null);
            }

            var user = new Register()
            {
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                Name = register.Name,
                UserName = register.Email,
                CreatedOn = DateTime.Now,
            };
            //var blobContainer = _blobServiceClient.GetBlobContainerClient("kabulexpress");
            //var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(register.ImageFile.FileName);



            //var blobClient = blobContainer.GetBlobClient(imageFileName);



            //// Delete the existing blob file if it exists
            //if (await blobClient.ExistsAsync())
            //{
            //    await blobClient.DeleteAsync();
            //}



            //await blobClient.UploadAsync(register.ImageFile.OpenReadStream());



            //user.ResumeUrl = blobClient.Uri.ToString(); // Save the URL instead of the file name
            var mail = new MailRequest()
            {
                Body = $"Please verify your account <a href='http://localhost:3000/emailConfirmation/{register.Name}'>Verify Emails</a>",
                Subject = "Verify Email Notification",
                ToEmail = register.Email
            };

            await sendemail(mail);
            var isSuccess = await _userManager.CreateAsync(user, register.Password);
            ProfessionalDetails professionalDetails = null; // Define professionalDetails variable

            if (isSuccess.Succeeded)
            {
                professionalDetails = new ProfessionalDetails();
                professionalDetails.RegiserID = user.Id;
                professionalDetails.Experience = register.Experience;
                professionalDetails.CurrentLocation = register.CurrentLocation;
                await _context.Professional_Details.AddAsync(professionalDetails);
                await _context.SaveChangesAsync();

                var education_Details = new EducationDetails();

                education_Details.RegisterID = user.Id;
                education_Details.HighestQualification = register.HighestQualification;
                education_Details.SelectYourField = register.SelectYourField;
                education_Details.University_Insitute = register.University_Insitute;
                education_Details.YearOfGraduation = register.YearOfGraduation;
                education_Details.EducationType = register.EducationType;
                await _context.Education_Details.AddAsync(education_Details);
                await _context.SaveChangesAsync();

                var jobprefrence = new JobPrefrence();
                jobprefrence.RegisterID = user.Id;
                jobprefrence.KeySkills = register.KeySkills;
                jobprefrence.Industry = register.Industry;
                jobprefrence.Department = register.Department;
                jobprefrence.PrefrredRole = register.PrefrredRole;
                jobprefrence.PrefrredLocation = register.PrefrredLocation;
                await _context.jobPrefrences.AddAsync(jobprefrence);
                await _context.SaveChangesAsync();

                bool sendSms = await _smsRepository.SendSMSPinWithBasicAuth(register.PhoneNumber);
                if (sendSms)
                {
                    return (true, null, null);
                }
                return (false, null, null);
            }

            if (isSuccess.Succeeded)
            {
                return (true, user, professionalDetails);
            }
            else
            {
                return (false, null, null);
            }
        }

        public async Task<List<Register>> GetAllRegistersAsync()
        {
            return await _context.Registers.ToListAsync();
        }

        public async Task<Register> GetRegisterById(string registerId)
        {
            return await _context.Registers.FindAsync(registerId);
        }

        public async Task<bool> UpdateRegister(Register register)
        {
            _context.Registers.Update(register);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteRegister(string id)
        {
            var register = await _context.Registers.FindAsync(id);
            if (register == null)
                return false;

            _context.Registers.Remove(register);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
