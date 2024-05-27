using Microsoft.EntityFrameworkCore;
using GyFChallenge.Models;

namespace GyFChallenge.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd();

                // Definir valor mínimo para Value
                tb.Property(col => col.Value)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired()
                    .HasDefaultValue(0);

                tb.Property(col => col.CreatedAt).IsRequired();
                tb.Property(col => col.Category).IsRequired();
            });

            modelBuilder.Entity<User>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd();

                tb.Property(col => col.Mail).HasMaxLength(100).IsRequired();
                tb.Property(col => col.Name).HasMaxLength(50).IsRequired();
                tb.Property(col => col.Surname).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
