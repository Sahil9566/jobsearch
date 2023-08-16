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
            var userExist = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (userExist is not null)
            {
                var educationalDetails = await _context.Education_Details.FirstOrDefaultAsync(x => x.RegisterID == userId);
                var professionalDetails = await _context.Professional_Details.FirstOrDefaultAsync(x => x.RegiserID == userId);
                var jobPrefrence = await _context.jobPrefrences.FirstOrDefaultAsync(x => x.RegisterID == userId);
                return Ok(new { userExist, educationalDetails, professionalDetails, jobPrefrence });
            }
            return BadRequest(new { message = "User Not Found" });
        }
    }
}
