using Microsoft.EntityFrameworkCore;
using StockInventorySync.Models;

namespace StockInventorySync.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => c.Category_Id);
            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Description).IsRequired();
            //modelBuilder.Entity<Category>().Property(c => c.DateCreated).IsRequired();
            //modelBuilder.Entity<Category>().Property(c => c.CreatedBy).IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Status).HasMaxLength(10);


			modelBuilder.Entity<Product>().HasKey(c => c.Product_Id);
			modelBuilder.Entity<Product>().Property(c => c.Name).IsRequired();
			modelBuilder.Entity<Product>().Property(c => c.Description).IsRequired();
			//modelBuilder.Entity<Category>().Property(c => c.DateCreated).IsRequired();
			//modelBuilder.Entity<Category>().Property(c => c.CreatedBy).IsRequired();
			modelBuilder.Entity<Product>().Property(c => c.Status).HasMaxLength(10);
            modelBuilder.Entity<Product>().HasOne(c => c.Category).WithMany(p => p.Products).HasForeignKey(c => c.Category_Id);
		}
    }
}
