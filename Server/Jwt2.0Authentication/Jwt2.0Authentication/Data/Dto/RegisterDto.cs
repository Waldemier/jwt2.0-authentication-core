using System.ComponentModel.DataAnnotations;

namespace Jwt2._0Authentication.Data.Dto
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare(nameof(Password))]
        public string Confirm { get; set; }
    }
}