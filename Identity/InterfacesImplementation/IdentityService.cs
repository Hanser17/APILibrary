using Application.Commom;
using Application.DTO_s.LoginDTO;
using Application.Interfaces.IdentityInterfaces;
using Azure.Core;
using Domain.Enums;
using Domain.JWTSettings;
using Identity.DBContext;
using Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.InterfacesImplementation
{
    public class IdentityService : IIdentityService
    {

        private readonly IdentityContext _context;
        private readonly UserManager<User> _userManager;
        private readonly JWT_Secrets _jwtSettings;
        public IdentityService(UserManager<User> userManager, IOptions<JWT_Secrets> settings, IdentityContext context)
        {
            _userManager = userManager;
            _jwtSettings = settings.Value;
            _context = context;

        }
        public async Task<AuthenticationResponse> AuthenticateAsync(Authenticationrequest dtoRequest)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(dtoRequest.UserName);
            if (user == null)
            {
                throw new APIExceptions("Usuario no encontrado", (int)HttpStatusCode.NotFound);
            }
            var result = await _userManager.CheckPasswordAsync(user, dtoRequest.PassWord);
            
            if (!result)
            {
                throw new APIExceptions("User not found with password provided", (int)HttpStatusCode.NotFound);
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            JwtSecurityToken jwtSecurityToken = await GenerateJWTokenAsync(user, userRoles);
            response.Id = user.Id;
            response.UserName = user.UserName;
            response.Email = user.Email;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.RefreshToken = GenerateRefreshToken().Token;
            var userRefreshToken = new UserRefreshToken
            {
                Token = response.RefreshToken,
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                UserId = user.Id
            };
            await SaveRefreshTocan(userRefreshToken);
            return response;
        }


        public async Task<AuthenticationResponse> GetRefreshTokenAsync(RefreshToken refreshToken)
        {
            {
                var principal = GetClaims(refreshToken.Token);

                if (principal == null)
                    throw new APIExceptions("Token inválido", 401);

                var email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;


                var user = await _userManager.FindByEmailAsync(email);
                var userRoles = await _userManager.GetRolesAsync(user);
                var TokenExist = await _context.UserRefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken.RefreshTaken);

                if (user == null || TokenExist == null || TokenExist.Expires <= DateTime.UtcNow)
                {
                    throw new APIExceptions("Refresh token inválido o expirado", 401);
                }
                else
                {
                    AuthenticationResponse response = new();
                    JwtSecurityToken jwtSecurityToken = await GenerateJWTokenAsync(user, userRoles);
                    response.UserName = user.UserName;
                    response.Email = user.Email;
                    response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    response.RefreshToken = GenerateRefreshToken().Token;

                    var userRefreshToken = new UserRefreshToken
                    {
                        Token = response.RefreshToken,
                        Expires = DateTime.UtcNow.AddDays(7),
                        Created = DateTime.UtcNow,
                        UserId = user.Id
                    };

                    await SaveRefreshTocan(userRefreshToken);
                    return response;
                }
            }
        }

        public async Task<JwtSecurityToken> GenerateJWTokenAsync(User user, IList<string> roles)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new List<Claim>
            {
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.AddRange(userClaims);

            claims.AddRange(
                roles.Select(role => new Claim(ClaimTypes.Role, role))
            );

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var sigingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: sigingCredentials
                );

            return jwToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,

            };
        }

        private string RandomTokenString()
        {
            using (var random = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[40];
                random.GetBytes(randomBytes);
                return BitConverter.ToString(randomBytes).Replace("-", "");
            }


        }

        private ClaimsPrincipal? GetClaims(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            }
            catch
            {
                return null;
            }
        }

        public async Task SaveRefreshTocan(UserRefreshToken refreshToken)
        {
            await _context.UserRefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }
    }
}
