using AsyncronousProgramming_MVC.Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace AsyncronousProgramming_MVC.Infrastructure.Context
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
            //Migrations safhasında "Microsoft.EntityFrameworkcore.Model.Validation[3000]" warning'i yememek için aşağıdaki işlemi yapıyoruz. 
            modelBuilder.Entity<Product>().Property(x => x.UnitPrice).HasColumnType("decimal");

            base.OnModelCreating(modelBuilder);
        }
    }
}
