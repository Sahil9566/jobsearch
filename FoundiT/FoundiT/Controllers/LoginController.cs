using Application.Repository.IRepository;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Infastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Cryptography;
using System.Text;

namespace FoundiT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly Applicationdbcontext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<Register> _userManager;
        private readonly ISmsRepository _smsRepository;
        public LoginController(IRegisterRepository registerRepository, Applicationdbcontext context, IMapper mapper, UserManager<Register> userManager, ISmsRepository smsRepository)
        {
            _registerRepository = registerRepository;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _smsRepository = smsRepository;

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTOs loginDTOs)
        {
            if (loginDTOs == null) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(loginDTOs.Email);

            if (user == null) return BadRequest("Email is not registered");

            var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTOs.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {

                var options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(7),
                    SameSite = SameSiteMode.None,
                    Secure = true
                };

                Response.Cookies.Append("UserEmail", user.Email, options);

                return Ok(new  {message = "User signed in successfully"  , userId=user.Id});
            }
            else if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                
                return BadRequest("Invalid email or password");
            }
            else
            {
                
                return BadRequest("Error in password verification");
            }
        }





    }
}
