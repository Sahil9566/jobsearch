using Application.Repository.IRepository;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Infastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Tsp;
using Sinch.ServerSdk.Callouts;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using  BCrypt;
using Microsoft.AspNetCore.Identity;
using Application.Repository;

namespace FoundiT.Controllers
{
    [Route("api/Regsiter")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly IMapper _mapper;
        private readonly Applicationdbcontext _context;
        private readonly UserManager<Register> _userManager;
        private readonly ISmsRepository _smsRepository;
        public RegisterController(IRegisterRepository registerRepository, IMapper mapper, Applicationdbcontext context, UserManager<Register> userManager, ISmsRepository smsRepository)
        {
            _registerRepository = registerRepository;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _smsRepository = smsRepository;
        }




        //display data from database
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var registers = await _registerRepository.GetAllRegistersAsync();
                return Ok(registers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while getting data");
            }
        }

        [HttpPost]
        [Route("api/RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterDTO register)
        {

         
            if (ModelState.IsValid)
            {
                var user = await _registerRepository.Register(register);

                if (user.Item1 == true)
                {
                    return Ok(user);
                }
                else
                {
                    return BadRequest(new { message = "Failed to Create" });
                }
            }
            return BadRequest(ModelState);
        }


        // get data by GetRegisterbyId
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetRegister(string registerId)
        {
            try
            {
                var register = await _registerRepository.GetRegisterById(registerId);

                if (register == null)
                {
                    return NotFound();
                }

                return Ok(register);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while getting data");
            }
        }


        [HttpPut("UpdateUserDetails")]
        public async Task<IActionResult> UpdateRegister([FromBody] Register register)
        {
            try
            {
                var existingRegister = await _registerRepository.GetRegisterById(register.Id);

                if (existingRegister == null)
                {
                    return NotFound();
                }

                existingRegister.Name = register.Name;
                _registerRepository.UpdateRegister(existingRegister);
                await _context.SaveChangesAsync();

                return Ok(existingRegister);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the register.");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRegister(string userId)
        {
            try
            {

                var register = _context.Registers.FirstOrDefault(x => x.Id == userId);

                if (register is null)
                {
                    return NotFound();
                }

                var isDeleted = await _userManager.DeleteAsync(register);
                return NoContent();
            }
            catch (Exception ex)
            {

                return StatusCode(500, "An error occurred while processing your request.");
            }
        }


        [HttpGet("VerifyEmail/{email}")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            var emailVerification = await _registerRepository.VerifyEmail(email);

            if (emailVerification)
            {
                return Ok(new { message = "Email verification successful" });
            }
            else
            {

                return BadRequest(new { message = "Email verification failed" });
            }
        }


        [HttpPost]
        [Route("api/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var existingUser = await _context.Registers.FirstOrDefaultAsync(u => u.Email == forgotPasswordDto.Email);
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (existingUser != null)
            {
                // Check if the new password and confirmation match
                //var passwordVerificationResult = await _userManager.CheckPasswordAsync(user, forgotPasswordDto.Password);
                if (forgotPasswordDto.Password == forgotPasswordDto.ConfirmPassword)
                {
                    // Hash the new password
                    //string hashedPassword = HashPassword(forgotPasswordDto.Password);

                    // Update the user's password
                    //existingUser.PasswordHash = hashedPassword;

                    // Save changes to the database
                    //_context.Update(existingUser);
                    //    await _userManager.UpdateAsync(user);
                    //var data=   await _userManager.ChangePasswordAsync(user, forgotPasswordDto.Password, forgotPasswordDto.ConfirmPassword);
                    //    await _context.SaveChangesAsync();
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,forgotPasswordDto.ConfirmPassword);
                    await _userManager.UpdateAsync(user);
                    return Ok(new { message = "Password Updated Successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Password and Confirm Password do not match" });
                }
            }
            else
            {
                return BadRequest(new { message = "Email is not registered" });
            }
        }

        [HttpPost("OTPVerfication")]
        public async Task<IActionResult> OTPVerfication(VerifyOTPVM param)
        {
            if (ModelState.IsValid)
            {
                var VerifyOtp = await _smsRepository.VerifyOTP(param);
                var user = _context.Registers.FirstOrDefault(x => x.PhoneNumber == param.PhoneNumber);

                if (VerifyOtp)
                {
                    user.PhoneNumberConfirmed = true;
                    _context.SaveChanges(); // Save the changes to the database

                    return Ok(new { message = "Successfully Verified" });
                }
                else
                {
                    return BadRequest(new { message = "Failed to Verify, Please check your phone number and OTP " });
                }
            }

            return BadRequest(ModelState);
        }


        //Resend OTp

        [HttpPost("Resend")]
        public async Task<IActionResult> SendSMS([FromBody] SMSRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string phoneNumber = request.PhoneNumber;

            bool success = await _smsRepository.SendSMSPinWithBasicAuth(phoneNumber);

            if (success)
            {
                return Ok(new { Message = "SMS sent successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to send SMS." });
            }
        }
    }
}
