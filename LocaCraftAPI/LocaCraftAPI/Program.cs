using LocaCraftAPI.LocaCraftAPI.Data;
using LocaCraftAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LocaCraftAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string corsName = "LocalDebugCors";
            const string databaseName = "RealEstateDb";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>( options =>
            {
                options.UseInMemoryDatabase(databaseName);
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

            app.UseCors(corsName);

            app.MapControllers();

            app.Run();
        }
    }
}
