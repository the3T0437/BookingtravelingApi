
using BookingTravelApi.Domains;
using BookingTravelApi.Infrastructure;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.Filters;
using System.Runtime.InteropServices;

namespace BookingTravelApi;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        var mySqlServerConnectionString = Environment.GetEnvironmentVariable("SqlServerConnectionString");

        if (!String.IsNullOrEmpty(mySqlServerConnectionString))
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(mySqlServerConnectionString)
            );
        }
        else
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );
        }

        builder.Services.AddControllers(); builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.UseSwagger(c => { c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0; });
        //     app.UseSwaggerUI(c => { c.SwaggerEndpoint("./v1/swagger.json", "MyServiceAPI"); });
        // }
        app.UseSwagger(c => { c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0; });
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("./v1/swagger.json", "MyServiceAPI"); });

        Directory.CreateDirectory(AppConfig.GetImagePath());
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(AppConfig.GetImagePath()),
            RequestPath = AppConfig.GetRequestImagePath()
        });

        app.UseExceptionHandler("/error");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
