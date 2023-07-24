using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Data
{
    public class Applicationdbcontext:IdentityDbContext<Register>
    {
        public Applicationdbcontext(DbContextOptions<Applicationdbcontext> options) : base(options)
        {
            Professional_Details = Set<ProfessionalDetails>();
            Education_Details= Set<EducationDetails>();
            jobPrefrences = Set<JobPrefrence>();
        }
        public DbSet<Register> Registers { get; set; }
        public DbSet<ProfessionalDetails> Professional_Details { get; }
        public DbSet<EducationDetails> Education_Details { get; set; }
        public DbSet<JobPrefrence>  jobPrefrences { get; set; }

    }
}
     