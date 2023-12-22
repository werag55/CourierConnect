using CourierCompanyApi.Models;
using Microsoft.EntityFrameworkCore;
using CourierConnect.DataAccess.Data.EntityTypeConfiguration;
using CourierCompanyApi.Data.EntityTypeConfiguration;

namespace CourierCompanyApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PersonalData> PersonalData { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new InquiryEntityTypeConfiguration().Configure(modelBuilder.Entity<Inquiry>());
            new AddressEntityTypeConfiguration().Configure(modelBuilder.Entity<Address>());
            new PackageEntityTypeConfiguration().Configure(modelBuilder.Entity<Package>());
            new PersonalDataEntityTypeConfiguration().Configure(modelBuilder.Entity<PersonalData>());
            new OfferEntityTypeConfiguration().Configure(modelBuilder.Entity<Offer>());
            new RequestEntityTypeConfiguration().Configure(modelBuilder.Entity<Request>());
            new DeliveryEntityTypeConfiguration().Configure(modelBuilder.Entity<Delivery>());
            new CourierEntityTypeConfiguration().Configure(modelBuilder.Entity<Courier>());
        }
    }
}