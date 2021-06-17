using FluentValidation;
using Jwt2._0Authentication.Data.Dto;

namespace Jwt2._0Authentication.FluentValidators
{
    public class RegisterDtoValidator: AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Name).MaximumLength(60).WithMessage("Must to be contain maximum 60 characters.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password)
                .MinimumLength(6)
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$")
                .WithMessage("Incorrect password. Must to be contain minimum 6 symbols. At least one uppercase letter, one lowercase letter and one number.");
        }
    }
}