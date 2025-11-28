using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using Microsoft.Extensions.Configuration;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Db Migration sadece relational ve connection string varsa çalışsın
            using (var scope = host.Services.CreateScope())
            {
                var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var connectionString = config.GetConnectionString("DefaultConnection");

                // Connection string yoksa migrate çalıştırma
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    var db = scope.ServiceProvider.GetRequiredService<ImsDbContext>();

                    // Relational veri tabanı mı kontrol et
                    if (db.Database.IsRelational())
                    {
                        db.Database.Migrate();
                    }
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
