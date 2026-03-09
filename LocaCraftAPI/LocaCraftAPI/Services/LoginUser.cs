using LocaCraftAPI.LocaCraftAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LocaCraftAPI.Services
{
    public class LoginUser
    {
        public record struct LoginRequest(string Email, string Password);

        public static void MapEndPoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/login", (LoginRequest request, UserManager<AppUser> userManager, IConfiguration configuration) =>
                Handle(request, userManager, configuration));
        }

        public static async Task<IResult> Handle(LoginRequest request, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, request.Password))
                return Results.Unauthorized();

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var jwtConfig = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]!));
            var expiration = DateTime.UtcNow.AddMinutes(double.Parse(jwtConfig["ExpirationInMinutes"]!));

            var token = new JwtSecurityToken(
                issuer: jwtConfig["Issuer"],
                audience: jwtConfig["Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Results.Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration
            });
        }
    }
}
