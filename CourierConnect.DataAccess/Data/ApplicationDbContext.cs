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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new InquiryEntityTypeConfiguration().Configure(builder.Entity<Inquiry>());
        }
    }
}
