using CourierCompanyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CourierCompanyApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<Offer> Offers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Offer>().HasData(
                new Offer
                {
                    Id = 1,
                    creationDate = DateTime.Now,
                    price = 100
                });
        }
    }
}