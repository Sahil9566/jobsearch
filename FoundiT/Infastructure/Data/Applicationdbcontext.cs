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

        }
        public DbSet<Register> Registers { get; set; }
    }
}
   