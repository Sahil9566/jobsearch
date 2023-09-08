using Domain.Models;
using Infastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoundiT.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Applicationdbcontext _context;
        public UserController(Applicationdbcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserDetailByUserId(string userId)
       
       {
            var user = new CombineTables();
            var data = await _context.Professional_Details.Include(a => a.register).FirstOrDefaultAsync();
            user.userExist = await _context.Users.Include(a=> a.EducationDetails).FirstOrDefaultAsync(x => x.Id == userId);
            if (user.userExist is not null)
            {
                user.educationaldetails = await _context.Education_Details.FirstOrDefaultAsync(x => x.RegisterID == userId);
                user.professionalDetails = await _context.Professional_Details.FirstOrDefaultAsync(x => x.RegiserID == userId);
                user.jobPrefrence = await _context.jobPrefrences.FirstOrDefaultAsync(x => x.RegisterID == userId);
                return Ok(user);
            }
            return BadRequest(new { message = "User Not Found" });
        }
    }
}
