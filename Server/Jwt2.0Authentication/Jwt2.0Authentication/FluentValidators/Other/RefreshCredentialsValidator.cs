using FluentValidation;
using Jwt2._0Authentication.Security.Models.Request;

namespace Jwt2._0Authentication.FluentValidators.Other
{
    public class RefreshCredentialsValidator: AbstractValidator<RefreshCredentials>
    {
        public RefreshCredentialsValidator()
        {
            RuleFor(x => x.JwtToken).NotEmpty();
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}