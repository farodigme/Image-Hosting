using Microsoft.EntityFrameworkCore;

namespace ImageHosting.Models.Entities
{
	public class ApplicationContext : DbContext
	{
		public DbSet<Storage> Storage { get; set; } = null!;

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Storage>()
						.ToTable("storage");

			base.OnModelCreating(modelBuilder);
		}
	}
}
