using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Repositories;
using LocaCraftAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
                    options.UseSqlite("Data Source=app.db");
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
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.FullName);
            });

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

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                if (db.Database.IsRelational() && db.Database.ProviderName!.Contains("Npgsql"))
                    db.Database.Migrate();
                else
                    db.Database.EnsureCreated();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!roleManager.RoleExistsAsync("User").GetAwaiter().GetResult())
                    roleManager.CreateAsync(new IdentityRole("User")).GetAwaiter().GetResult();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    config.RoutePrefix = string.Empty;
                });
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
