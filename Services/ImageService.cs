using ImageHosting.Models.DTOs.Upload;
using ImageHosting.Models.Entities;
using SixLabors.ImageSharp;
using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Services
{
	public class ImageService
	{
		private ApplicationContext _context;
		private ImageValidator _validator;
		public ImageService(ApplicationContext context, ImageValidator validator)
		{
			_context = context;
			_validator = validator;
		}

		public async Task<ImageUploadResponse> UploadImageAsync(ImageUploadRequest request)
		{
			var validation = await _validator.ValidateAsync(request.ImageFile, new Models.Settings.ImageValidationSettings());

			if (!validation.IsValid)
			{
				return new ImageUploadResponse()
				{
					Success = false,
					Error = validation.Error
				};
			}

			var guid = Guid.NewGuid().ToString();
			var folder = guid.Split('-')[0]; 
			var folderPath = Path.Combine("var", "www", "images", folder);
			Directory.CreateDirectory(folderPath);

			var fileName = guid + ".jpg";
			var fullPath = Path.Combine(folderPath, fileName);

			using (var image = await SixLabors.ImageSharp.Image.LoadAsync(request.ImageFile.OpenReadStream()))
			{
				await image.SaveAsJpegAsync(fullPath);
			}

			var newImage = new Models.Entities.Storage()
			{
				Guid = guid.ToString(),
				Path = fullPath,
			};

			await _context.Storage.AddAsync(newImage);
			await _context.SaveChangesAsync();

			return new ImageUploadResponse()
			{
				Success = true,
				NativeImageUrl = newImage.Guid,
				ThumbnailImageUrl = newImage.Guid + "_thumb"
			};
		}
	}
}
