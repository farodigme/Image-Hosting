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
		public async Task<IActionResult> Upload([FromForm] ImageUploadRequest request)
		{
			var result = await _imageService.UploadImageAsync(request);
			return result.Success ? Ok(result) : BadRequest(result);
		}

		[HttpGet("get/{guid}")]
		public async Task<IActionResult> Get([FromRoute] string guid)
		{
			var stream = await _imageService.GetImageStreamAsync(guid);
			return stream != null ? File(stream, "image/jpeg") : BadRequest();
		}
	}
}
