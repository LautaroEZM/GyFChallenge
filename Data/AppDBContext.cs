using Microsoft.EntityFrameworkCore;
using GyFChallenge.Models;
namespace GyFChallenge.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }
        //Set de las tablas.
        public DbSet <Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        //Sobreescritura para poder aplicar la configuración.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(tb =>
            {
                tb.HasKey(col => col.Id);
                tb.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd(); //Define que la key es autoincrementable al agregar valores.


                //DEFINIR VALOR MINIMO PARA VALUE
                });
            

            modelBuilder.Entity<User>(tb =>
            {
                tb.HasKey(col => col.Mail);
                tb.Property(col => col.Mail).HasMaxLength(50);

                tb.Property(col => col.Name).HasMaxLength(50);

                tb.Property(col => col.Surname).HasMaxLength(50);
            });

            //Define las clases como tablas.
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Product>().ToTable("Users");

        }
        
    }
}
