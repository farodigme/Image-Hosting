namespace ImageHosting.Models.Entities
{
	public class Storage
	{
		public int Id { get; set; }
		public string Guid { get; set; }
		public string Path { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}
}
