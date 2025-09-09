using Microsoft.EntityFrameworkCore;
using WebShop.Models;
using WebShop.Models.Abstract;
using Microsoft.Extensions.Configuration;

namespace WebShop.Data
{
    public class ShopDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Processor> Processors { get; set; }
        public DbSet<ThermalPaste> ThermalPastes { get; set; }

        private IConfiguration Configuration { get; init; }

        public ShopDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Processor>().ToTable("Processor");
            modelBuilder.Entity<ThermalPaste>().ToTable("ThermalPastes");

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("money");

            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .IsRequired(false);
        }
    }
}
