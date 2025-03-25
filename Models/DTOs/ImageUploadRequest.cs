using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Models.DTOs
{
	public class ImageUploadRequest
	{
		[Required]
		public string Type { get; set; }

		[Required]
		public IFormFile ImageFile { get; set; }
	}
}
