namespace Jwt2._0Authentication.Security.Models.Request
{
    public class RefreshCredentials
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}