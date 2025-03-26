namespace ImageHosting.Models.DTOs.Upload
{
	public class ImageUploadResponse
	{
		public bool Success { get; set; }
		public string? Error { get; set; }
		public string NativeImageUrl { get; set; }
		public string ThumbnailImageUrl { get; set; }
	}
}
