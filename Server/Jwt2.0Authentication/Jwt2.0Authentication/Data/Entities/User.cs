using System.ComponentModel.DataAnnotations;
using Jwt2._0Authentication.Data.Enums;

namespace Jwt2._0Authentication.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public RoleTypes Role { get; set; }
    }
}