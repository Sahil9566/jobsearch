using Application.IRepository;
using AutoMapper;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FoundItApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterRepository _registerRepository;
        private readonly IMapper _mapper;
        private readonly IImageStorageService _imageStorageService;

        public RegisterController(IRegisterRepository registerRepository, IMapper mapper, IImageStorageService imageStorageService)
        {
            _registerRepository = registerRepository;
            _mapper = mapper;
            _imageStorageService = imageStorageService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Registerdtos registerDto, IFormFile imageFile)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    
                    using (var stream = imageFile.OpenReadStream())
                    {
                        string imageName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                        string imageUrl = await _imageStorageService.UploadImageAsync(stream, imageName);
                        registerDto.UploadResume = imageFile; 
                    }
                }

                var register = _mapper.Map<Register>(registerDto);

                var createdRegister = await _registerRepository.Create(register);

                var createdRegisterDto = _mapper.Map<Registerdtos>(createdRegister);

                return Ok(createdRegisterDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating register.");
            }
        }
    }
}
