using LocaCraftAPI.LocaCraftAPI.Data;
using Microsoft.AspNetCore.Identity;

namespace LocaCraftAPI.Services
{
    public class RegisterUser
    {
        const string defaultRole = "User";

        public record struct Request(string Email, string Password);

        public static void MapEndPoint(IEndpointRouteBuilder app)
        {
            app.MapPost("api/register", (Request request, AppDbContext dbContext, UserManager<AppUser> userManager) =>
                Handle(request, dbContext, userManager));
        }

        public static async Task<IResult> Handle(Request request, AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            using var transaction = await dbContext.Database.BeginTransactionAsync();

            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                await transaction.RollbackAsync();
                return Results.BadRequest(result.Errors);
            }

            IdentityResult addToRoleResult = await userManager.AddToRoleAsync(user, defaultRole);
            if (!addToRoleResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return Results.BadRequest(addToRoleResult.Errors);
            }

            await transaction.CommitAsync();
            return Results.Ok(new { user.Id, user.Email });
        }
    }
}
