using System;

namespace Jwt2._0Authentication.Security.Configuration
{
    public class JwtConfig
    {
        public string SecurityKey { get; set; }
        public TimeSpan ExpirationTime { get; set; }
    }
}