using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>( options =>
            {
                options.UseInMemoryDatabase("RealEstateDb");
            });

            builder.Services.AddScoped<IRealEstateAssetRepository, RealEstateAssetRepository>();

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(config =>
                {
                    config.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    config.RoutePrefix = string.Empty;
                });
            }

            app.MapGet("/", () => "Hello World!");

            app.Run();
        }
    }
}
