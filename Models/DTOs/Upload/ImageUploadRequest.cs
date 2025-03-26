using System.ComponentModel.DataAnnotations;

namespace ImageHosting.Models.DTOs.Upload
{
    public class ImageUploadRequest
    {
        [Required]
        public IFormFile ImageFile { get; set; }
    }
}
