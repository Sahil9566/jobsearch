using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FoundiT.DTOMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDTO, Register>().ReverseMap();
            CreateMap<LoginDTOs, Register>().ReverseMap();
        }
    }
}   
     