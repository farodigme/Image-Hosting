namespace ImageHosting.Models.Settings
{
	public class ImageValidationSettings
	{
		public long MaxBytes { get; set; } = 5 * 1024 * 1024;
		public int MinWidth { get; set; } = 0;
		public int MinHeight { get; set; } = 0;
		public string[] AllowedExtensions { get; set; } = [".jpg", ".jpeg", ".png"];
		public string[] AllowedMimeTypes { get; set; } = ["image/jpeg", "image/png"];
	}
}
