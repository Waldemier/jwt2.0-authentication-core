using Jwt2._0Authentication.Data.Entities;
using Jwt2._0Authentication.Data.Entities.Other;
using Microsoft.EntityFrameworkCore;

namespace Jwt2._0Authentication.Data
{
    public interface IAppDbContext
    { 
        DbSet<User> Users { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        void SaveChanges();
    }
}