using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jwt2._0Authentication.Data.Entities.Other
{
    public class RefreshToken
    {
        // Actually we can add an Jwt token Id (Jti) for addition security
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}