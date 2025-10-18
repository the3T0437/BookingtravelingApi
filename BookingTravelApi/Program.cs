using BookingTravelApi.Domains;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql; // 👈 cần thêm package này
using DotNetEnv;

namespace BookingTravelApi;

public class Program
{
    public static void Main(string[] args)
    {
        // Load .env file
        Env.Load();

        var builder = WebApplication.CreateBuilder(args);

        // ✅ Đọc đúng connection string theo chuẩn ASP.NET
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        // ✅ Dùng MySQL provider (Pomelo)
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString)
            )
        );

        // Add services to the container
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

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
