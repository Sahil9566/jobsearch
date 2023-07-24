using Application.Repository.IRepository;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Infastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Identity;

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
        public RegisterController(IRegisterRepository registerRepository, IMapper mapper, Applicationdbcontext context, UserManager<Register> userManager)
        {
            _registerRepository = registerRepository;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
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
            
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user != null)
            {
                if (forgotPasswordDto.Password == forgotPasswordDto.ConfirmPassword)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user,forgotPasswordDto.ConfirmPassword);
                    await _userManager.UpdateAsync(user);
                    return Ok(new { message = "Password Updated Successfully" });
                }
                else
                {
                    return BadRequest(new { message = "Password and Confirm Password do not match" });
                }
            }
          return BadRequest(new { message = "Email is not registered" });
            
        }

     
     

    }
}
