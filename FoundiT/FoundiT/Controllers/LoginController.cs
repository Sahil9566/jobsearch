using Application.Repository.IRepository;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Infastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

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
        public LoginController(IRegisterRepository registerRepository, Applicationdbcontext context, IMapper mapper, UserManager<Register> userManager)
        {
            _registerRepository = registerRepository;
            _context = context;
            _mapper = mapper;
            _userManager = userManager;

        }



        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDTOs loginDTOs)
        {
            if (loginDTOs == null) return BadRequest(ModelState);

            var user = _context.Registers.FirstOrDefault(x => x.Email == loginDTOs.Email);

            if (user == null) return BadRequest("Email is not registered");

            var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDTOs.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid email or password");
            }

            if (!user.EmailConfirmed)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = "Email is not verified" });
            }

            return Ok(new { message = "User signed in successfully" });
        }

    }
}
