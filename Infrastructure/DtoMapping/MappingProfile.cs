using AutoMapper;
using Domain.Dtos;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.DtoMapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Registerdtos, Register>().ReverseMap();
        }
    }
}
