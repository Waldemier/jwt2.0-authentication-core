using System;

namespace Jwt2._0Authentication.Security.Models.Responce
{
    public class AuthenticatedResponse
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshExpiryTime { get; set; }
    }
}