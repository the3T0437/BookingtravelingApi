
using System.Runtime.InteropServices;
using BookingTravelApi.Domains;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace BookingTravelApi;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("ConnectionString"))
        );
        builder.Services.AddControllers(); builder.Services.AddEndpointsApiExplorer();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler("/error");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
