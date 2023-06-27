﻿using Application.Repository.IRepository;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
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
        public async Task<IActionResult> RegisterUser(RegisterDTO register)
        {
            if (ModelState.IsValid)
            {
                //var Mapping = _mapper.Map<RegisterDTO, Register>(register);
                var user = await _registerRepository.Register(register);
                if (user.Item1 == true)
                {
                    return Ok(user);
                }
                else
                    return BadRequest(new { message = "Failed to Create" });
                //var createdRegisterDto = _mapper.Map<Register>(registerUser);


            }
            return BadRequest(ModelState);
        }
    }
}