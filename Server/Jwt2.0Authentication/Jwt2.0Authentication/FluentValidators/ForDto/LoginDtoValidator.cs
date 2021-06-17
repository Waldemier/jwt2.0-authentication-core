using FluentValidation;
using Jwt2._0Authentication.Data.Dto;

namespace Jwt2._0Authentication.FluentValidators
{
    public class LoginDtoValidator: AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Email field must be not an empty.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password field must be not an empty and had minimum 6 symbols.");
        }
    }
}