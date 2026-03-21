using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Repositories;
using LocaCraftAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

namespace LocaCraftAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string corsName = "LocalDebugCors";

            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                if (!string.IsNullOrEmpty(connectionString))
                    options.UseNpgsql(connectionString);
                else
                    options.UseSqlite("Data Source=app.db")
                           .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsName, builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddScoped<IRealEstateAssetRepository, RealEstateAssetRepository>();
            builder.Services.AddScoped<ITenantRepository, TenantRepository>();
            builder.Services.AddScoped<ILeaseRepository, LeaseRepository>();
            builder.Services.AddScoped<ILessorRepository, LessorRepository>();

            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient<IInseeService, InseeService>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddIdentityCore<AppUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            var jwtConfig = builder.Configuration.GetSection("Jwt");
            if (string.IsNullOrEmpty(jwtConfig["Key"]))
                throw new InvalidOperationException(
                    "JWT key is not configured. Set 'Jwt:Key' in appsettings.Development.json or via environment variable 'Jwt__Key'.");

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtConfig["Issuer"],
                        ValidAudience = jwtConfig["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtConfig["Key"]!))
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                if (builder.Configuration.GetValue<bool>("TRUST_PROXY"))
                {
                    // Container is only reachable by nginx (not publicly exposed), so trust any proxy.
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                }
            });

            // Add OpenAPI/Swagger support
            // Learn more about configuring OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddOpenApi();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!roleManager.RoleExistsAsync("User").GetAwaiter().GetResult())
                    roleManager.CreateAsync(new IdentityRole("User")).GetAwaiter().GetResult();
            }

            app.UseForwardedHeaders();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference("/docs", options =>
                {
                    options.Title ="LocaCraft API Reference";
                    options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);                  
                });
                app.MapGet("/", () => Results.Redirect("/docs/v1")).ExcludeFromDescription();
                app.MapGet("/index.html", () => Results.Redirect("/docs/v1")).ExcludeFromDescription();
            }

            app.UseCors(corsName);

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            

            RegisterUser.MapEndPoint(app);
            LoginUser.MapEndPoint(app);

            app.Run();
        }
    }
}
