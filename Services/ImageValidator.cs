using ImageHosting.Models.Settings;
using SixLabors.ImageSharp;

namespace ImageHosting.Services
{
	public class ImageValidator
	{
		public async Task<(bool IsValid, string? Error)> ValidateAsync(IFormFile file, ImageValidationSettings settings)
		{
			if (file == null || file.Length == 0)
				return (false, "Файл не передан.");

			if (file.Length > settings.MaxBytes)
				return (false, $"Размер не должен превышать {settings.MaxBytes / 1024 / 1024}MB.");

			var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
			if (!settings.AllowedExtensions.Contains(ext))
				return (false, "Недопустимое расширение.");

			if (!settings.AllowedMimeTypes.Contains(file.ContentType))
				return (false, "Недопустимый MIME-тип.");

			try
			{
				using var image = await Image.LoadAsync(file.OpenReadStream());
				if (image.Width < settings.MinWidth || image.Height < settings.MinHeight)
					return (false, $"Минимальное разрешение: {settings.MinWidth}x{settings.MinHeight}px.");
			}
			catch
			{
				return (false, "Файл не является допустимым изображением.");
			}

			return (true, null);
		}
	}
}
