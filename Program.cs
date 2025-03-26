using ImageHosting.Models.Entities;
using ImageHosting.Models.Settings;
using ImageHosting.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ImageHosting
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<ApplicationContext>(options =>
			{
				options.UseMySql(
					builder.Configuration.GetConnectionString("DefaultConnection"),
					new MySqlServerVersion(new Version(8, 0, 32))
					);
			});

			builder.Services.AddScoped<ImageService>();
			builder.Services.AddSingleton<ImageValidator>();

			builder.Services.AddControllers();

            builder.Services.Configure<ImageValidationSettings>(builder.Configuration.GetSection("ImageValidation"));

            var app = builder.Build();

            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }
    }
}
