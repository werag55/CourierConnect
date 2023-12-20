using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CourierConnect.Models;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using CourierConnect.DataAccess.Data.EntityTypeConfiguration;

namespace CourierConnect.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Inquiry> Inquiries { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PersonalData> PersonalData { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new InquiryEntityTypeConfiguration().Configure(builder.Entity<Inquiry>());
            new AddressEntityTypeConfiguration().Configure(builder.Entity<Address>());
            new PackageEntityTypeConfiguration().Configure(builder.Entity<Package>());
            new PersonalDataEntityTypeConfiguration().Configure(builder.Entity<PersonalData>());
            new OfferEntityTypeConfiguration().Configure(builder.Entity<Offer>());
            new RequestEntityTypeConfiguration().Configure(builder.Entity<Request>());
            new DeliveryEntityTypeConfiguration().Configure(builder.Entity<Delivery>());
        }
    }
}
