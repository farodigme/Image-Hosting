using ImageHosting.Models.DTOs.Upload;
using ImageHosting.Models.Entities;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;

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
			var validation = await _validator.ValidateAsync(request.Image, new Models.Settings.ImageValidationSettings());

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
			var folderPath = Path.Combine("www", "images", folder);
			Directory.CreateDirectory(folderPath);

			var fileName = guid + ".jpg";
			var fullPath = Path.Combine(folderPath, fileName);

			using (var image = await SixLabors.ImageSharp.Image.LoadAsync(request.Image.OpenReadStream()))
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

		public async Task<Stream?> GetImageStreamAsync(string guid)
		{
			var isThumb = guid.EndsWith("_thumb");
			var pureGuid = isThumb ? guid[..^6] : guid;

			var image = await _context.Storage.FirstOrDefaultAsync(i => i.Guid == pureGuid);
			if (image == null)
				return null;

			string fullPath = Path.Combine(Environment.CurrentDirectory, image.Path);
			if (!File.Exists(fullPath))
				return null;

			if (isThumb)
			{
				var originalImage = await Image.LoadAsync(fullPath);

				originalImage.Mutate(x => x.Resize(new ResizeOptions
				{
					Size = new Size(300, 300),
					Mode = ResizeMode.Crop
				}));

				var memStream = new MemoryStream();
				await originalImage.SaveAsJpegAsync(memStream, new JpegEncoder { Quality = 85 });
				memStream.Position = 0;
				return memStream;

			}
			else
			{
				var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
				return stream;
			}
		}
	}
}
