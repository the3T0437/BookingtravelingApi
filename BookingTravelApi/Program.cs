
using System.Runtime.InteropServices;
using BookingTravelApi.Domains;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

namespace BookingTravelApi;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //options.UseSqlServer(connectionString)
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );

        builder.Services.AddControllers(); builder.Services.AddEndpointsApiExplorer();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddSwaggerGen(c =>
        {
            //     c.ExampleFilters();
        });
        // // Register example providers
        // builder.Services.AddSwaggerExamplesFromAssemblyOf<CreateTourFormExample>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger(c => { c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0; });
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("./v1/swagger.json", "MyServiceAPI"); });
        }

        app.UseExceptionHandler("/error");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
