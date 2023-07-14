
using Domain.DTOs;
using FluentValidation;


namespace Domain.Models
{
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(u => u.ImageFile).NotNull().WithMessage("Please ensure that you have entered your ImageFile");
            RuleFor(u => u.Gender).NotNull();
            RuleFor(u => u.Name).NotNull().WithMessage("Name is required");
            RuleFor(u => u.Email).NotNull().WithMessage("Email is required");
            RuleFor(u => u.Password).NotNull().MaximumLength(10);
            RuleFor(u => u.PhoneNumber).NotNull();


        }
    }
}
