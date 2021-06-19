using System;
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
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;

namespace Jwt2._0Authentication.Security
{
    public class JwtService: IJwtService
    {
        private readonly JwtConfig _jwtConfig;
        private readonly IAppDbContext _context;
        private readonly IConfiguration _configuration;
        public JwtService(JwtConfig jwtConfig, IAppDbContext context, IConfiguration configuration)
        {
            this._jwtConfig = jwtConfig;
            this._context = context;
            this._configuration = configuration;
        }

        
        /// <summary>
        /// Basic jwt and refresh tokens generating method, which executes after authentication (login) in system. 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns the new jwt and refresh tokens for a user.</returns>
        public AuthenticatedResponse Authenticate(User user)
        {
            var jwtToken = this.GenerateToken(user);
            var refreshToken = this.GenerateRefreshToken();

            if (this._context.RefreshTokens.Any(x => x.UserId == user.Id))
            {
                var refreshTokenModel = this._context.RefreshTokens.FirstOrDefault(x => x.UserId == user.Id);
                refreshTokenModel.Token = refreshToken;
                refreshTokenModel.ExpiryTime =
                    DateTime.UtcNow.AddDays(int.Parse(this._configuration["RefreshTokenExpirationDays"]));
                this._context.RefreshTokens.Update(refreshTokenModel);
                this._context.SaveChanges();
            }
            else
            {
                this._context.RefreshTokens.Add(new RefreshToken()
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpiryTime = DateTime.UtcNow.AddDays(int.Parse(this._configuration["RefreshTokenExpirationDays"]))
                });
                this._context.SaveChanges();
            }


            return new AuthenticatedResponse 
            {
                JwtToken = jwtToken, 
                RefreshToken = refreshToken,
                RefreshExpiryTime = DateTime.UtcNow.AddDays(int.Parse(this._configuration["RefreshTokenExpirationDays"])) // The same as in db refresh token expiry time
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
                refreshTokenModel.ExpiryTime = DateTime.UtcNow.AddDays(int.Parse(this._configuration["RefreshTokenExpirationDays"]));
                this._context.RefreshTokens.Update(refreshTokenModel);
                this._context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException("Refresh token for current user does not exist.");
            }

            return new AuthenticatedResponse
            {
                JwtToken = jwtToken, 
                RefreshToken = refreshToken,
                RefreshExpiryTime = DateTime.UtcNow.AddDays(int.Parse(this._configuration["RefreshTokenExpirationDays"])) // The same as in db refresh token expiry time
            };
        }
        
        /// <summary>
        /// Refresh tokens method.
        /// </summary>
        /// <param name="refreshCredentials"></param>
        /// <returns>Returns a method which generates and returns the new jwt and refresh tokens.</returns>
        /// <exception cref="SecurityTokenException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
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

            // if the current refresh token has not stored in the database
            if (!this._context.RefreshTokens.Any(x => x.Token == refreshCredentials.RefreshToken))
            {
                throw new SecurityTokenException("Invalid token passed.");
            }
            
            // get refresh token from db for any validations below 
            var refreshTokenModelFromDb =
                this._context.RefreshTokens.SingleOrDefault(x => x.Token == refreshCredentials.RefreshToken);
            
            // get user from db for any validations below
            var user = this._context.Users.SingleOrDefault(x => x.Email == userEmail);
            if (user == null)
            {
                throw new ArgumentNullException("User with current email does not exist.");
            }
            
            // if the current refresh token does not apply to this user => throw exception
            if (refreshTokenModelFromDb.UserId != user.Id)
            {
                throw new SecurityTokenException("Invalid token.");
            }
            
            // if the token expiry time less than the current date => delete token from db and throw exception
            var refreshTokenExpiryTime = refreshTokenModelFromDb.ExpiryTime;
            if (refreshTokenExpiryTime < DateTime.UtcNow)
            {
                this._context.RefreshTokens.Remove(refreshTokenModelFromDb);
                this._context.SaveChanges();
                
                throw new SecurityTokenException("Expiration time has expired.");
            }

            return this.Authenticate(user, principals.Claims.ToArray());
        }
        
        /// <summary>
        /// Jwt token generation method. 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claims"></param>
        /// <returns>Returns a new jwt token with specify claims.</returns>
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

            var token = tokenHandler.CreateToken(tokenDescriptor); // Jwt encoding

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Refresh token generating method.
        /// </summary>
        /// <returns>Returns a random string which will be used as refresh token.</returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            
            return Convert.ToBase64String(randomNumber);
        }
    }
}