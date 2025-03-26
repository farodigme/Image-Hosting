using ImageHosting.Models.DTOs.Upload;
using ImageHosting.Models.Entities;
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

			Guid guid = Guid.NewGuid();

			string path = "Images/" + guid.ToString();
			
			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				await request.ImageFile.CopyToAsync(fileStream);
			}

			var newImage = new Image()
			{
				Guid = guid.ToString(),
				Path = path,
			};

			_context.Images.Add(newImage);
			_context.SaveChanges();

			return new ImageUploadResponse()
			{
				Success = true,
				NativeImageUrl = newImage.Guid,
				ThumbnailImageUrl = newImage.Guid + "_thumb"
			};
		}


	}
}
