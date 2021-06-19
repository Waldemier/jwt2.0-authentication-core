using System;
using Jwt2._0Authentication.Data.Entities;
using Jwt2._0Authentication.Security.Models.Request;
using Jwt2._0Authentication.Security.Models.Responce;

namespace Jwt2._0Authentication.Security
{
    public interface IJwtService
    {
        AuthenticatedResponse Authenticate(User user);
        AuthenticatedResponse Refresh(RefreshCredentials refreshCredentials);
    }
}