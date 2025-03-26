using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Models.Entities
{
	public class ApplicationContext : DbContext
	{
		public DbSet<Image> Images { get; set; } = null!;

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Image>()
						.ToTable("images");

			base.OnModelCreating(modelBuilder);
		}
	}
}
