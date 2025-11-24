using BookingTravelApi.Domains;
using BookingTravelApi.Infrastructure;
using BookingTravelApi.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using PayOS;
using PayOS.Models.V1.Payouts;

namespace BookingTravelApi;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        var sqlServerConnectionString = Environment.GetEnvironmentVariable("SqlServerConnectionString");
        var mySqlConnectionString = Environment.GetEnvironmentVariable("DBEverConnectionString");

        if (!String.IsNullOrEmpty(mySqlConnectionString))
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString))
            );
        }
        else if (!String.IsNullOrEmpty(sqlServerConnectionString))
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(sqlServerConnectionString)
            );
        }
        else
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );
        }

        builder.Services.AddSingleton<PayOSClient>(op =>
        {
            var clientId = Environment.GetEnvironmentVariable("ClientId");
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");
            var checksumKey = Environment.GetEnvironmentVariable("ChecksumKey");
            PayOS.PayOSClient payOS = new PayOSClient(clientId, apiKey, checksumKey);

            return payOS;
        });
        builder.Services.AddScoped<PaymentService>();
        builder.Services.AddScoped<ChangeStatusBookingService>();

        builder.Services.AddControllers(); builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();


        // Đăng ký MailService
        builder.Services.AddTransient<MailService>();

        // Đăng ký push notification
        builder.Services.AddSingleton<FirebaseNotificationService>();

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

        // app.UseExceptionHandler("/error");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
