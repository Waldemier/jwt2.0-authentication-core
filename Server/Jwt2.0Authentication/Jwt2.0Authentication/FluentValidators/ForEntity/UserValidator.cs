using FluentValidation;
using Jwt2._0Authentication.Data.Entities;

namespace Jwt2._0Authentication.FluentValidators
{
    public class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Name).MaximumLength(60);
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).MinimumLength(6).NotEmpty();
        }
    }
}