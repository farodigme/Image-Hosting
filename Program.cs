using ImageHosting.Interfaces;
using ImageHosting.Models.Entities;
using ImageHosting.Models.Settings;
using ImageHosting.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

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

			builder.Services.AddControllers();
			builder.Services.AddScoped<ImageService>();
			builder.Services.AddSingleton<ImageValidator>();
			builder.Services.AddHttpContextAccessor();
			builder.Services.AddScoped<IUrlService, UrlService>();

			builder.Services.Configure<ImageValidationSettings>(builder.Configuration.GetSection("ImageValidation"));

			var app = builder.Build();

			app.MapControllers();
			app.Run();
		}
	}
}
