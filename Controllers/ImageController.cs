using ImageHosting.Models.DTOs.Upload;
using ImageHosting.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImageHosting.Controllers
{
	[ApiController]
	[Route("api/{controller}")]
	public class ImageController : Controller
	{
		private ImageService _imageService;

		public ImageController(ImageService imageService)
		{
			_imageService = imageService;
		}

		[HttpPost("upload")]
		public async Task<IActionResult> Upload(ImageUploadRequest request)
		{
			var result = await _imageService.UploadImageAsync(request);
			return result.Success ? Ok(result) : BadRequest(result);
		}
	}
}
