using Application.Repository.IRepository;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoundiT.Controllers
{
    [Route("api/Regsiter")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly IMapper _mapper;
        public RegisterController(IRegisterRepository registerRepository, IMapper mapper)
        {
            _registerRepository = registerRepository;
            _mapper = mapper;   
        }
        [HttpPost]
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

    }
}
