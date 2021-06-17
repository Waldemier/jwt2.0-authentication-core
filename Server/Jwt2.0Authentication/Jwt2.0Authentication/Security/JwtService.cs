using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Jwt2._0Authentication.Data;
using Jwt2._0Authentication.Data.Entities;
using Jwt2._0Authentication.Data.Entities.Other;
using Jwt2._0Authentication.Security.Configuration;
using Jwt2._0Authentication.Security.Models.Request;
using Jwt2._0Authentication.Security.Models.Responce;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;

namespace Jwt2._0Authentication.Security
{
    public class JwtService: IJwtService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IAppDbContext _context;

        public JwtService(JwtConfig jwtConfig, IAppDbContext context)
        {
            this._jwtConfig = jwtConfig;
            this._context = context;
        }

        public AuthenticatedResponse Authenticate(User user)
        {
            var jwtToken = this.GenerateToken(user);
            var refreshToken = this.GenerateRefreshToken();

            if (this._context.RefreshTokens.Any(x => x.UserId == user.Id))
            {
                var refreshTokenModel = this._context.RefreshTokens.FirstOrDefault(x => x.UserId == user.Id);
                refreshTokenModel.Token = refreshToken;
                this._context.RefreshTokens.Update(refreshTokenModel);
                this._context.SaveChanges();
            }
            else
            {
                this._context.RefreshTokens.Add(new RefreshToken()
                {
                    UserId = user.Id,
                    Token = refreshToken,
                });
                this._context.SaveChanges();
            }
            // if (this.UsersRefreshTokens.ContainsKey(user.Email))
            // {
            //     this.UsersRefreshTokens[user.Email] = refreshToken;
            // }
            // else
            // {
            //     this.UsersRefreshTokens.Add(user.Email, refreshToken);
            // }
            
            return new AuthenticatedResponse
            {
                JwtToken = jwtToken, 
                RefreshToken = refreshToken
            };
        }
        
        /// <summary>
        /// Method which executes after refresh token method.
        /// </summary>
        private AuthenticatedResponse Authenticate(User user, Claim[] claims)
        {
            var jwtToken = this.GenerateToken(user, claims);
            var refreshToken = this.GenerateRefreshToken();

            if (this._context.RefreshTokens.Any(x => x.UserId == user.Id))
            {
                var refreshTokenModel = this._context.RefreshTokens.FirstOrDefault(x => x.UserId == user.Id);
                refreshTokenModel.Token = refreshToken;
                this._context.RefreshTokens.Update(refreshTokenModel);
                this._context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException();
            }
            
            // if (UsersRefreshTokens.ContainsKey(user.Email))
            // {
            //     this.UsersRefreshTokens[user.Email] = refreshToken;
            // }
            // else
            // {
            //     this.UsersRefreshTokens.Add(user.Email, refreshToken);
            // }
            
            return new AuthenticatedResponse
            {
                JwtToken = jwtToken, 
                RefreshToken = refreshToken
            };
        }

        public AuthenticatedResponse Refresh(RefreshCredentials refreshCredentials)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principals = tokenHandler.ValidateToken(
                refreshCredentials.JwtToken,
                new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._jwtConfig.SecurityKey)),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = false,
                }, 
                out SecurityToken validatedToken);
            
            var jwtToken = validatedToken as JwtSecurityToken;
            
            if (jwtToken == null || jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature,
                StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token passed.");
            }

            var userEmail = principals.Identity.Name;

            if (!this._context.RefreshTokens.Any(x => x.Token == refreshCredentials.RefreshToken))
            {
                throw new SecurityTokenException("Invalid token passed.");
            }
            
            // if (refreshCredentials.RefreshToken != this.UsersRefreshTokens[userEmail])
            // {
            //     throw new SecurityTokenException("Invalid token passed.");
            // }

            var user = this._context.Users.SingleOrDefault(x => x.Email == userEmail);
            if (user == null)
            {
                throw new ArgumentNullException();
            }
            
            return this.Authenticate(user, principals.Claims.ToArray());
        }

        private string GenerateToken(User user, Claim[] claims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                    claims ?? new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.GetDisplayName())
                }),
                Expires = DateTime.UtcNow.Add(this._jwtConfig.ExpirationTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            
            return Convert.ToBase64String(randomNumber);
        }
    }
}