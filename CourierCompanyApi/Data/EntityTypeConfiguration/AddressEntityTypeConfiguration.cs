using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourierCompanyApi.Models;

namespace CourierConnect.DataAccess.Data.EntityTypeConfiguration
{
    public class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.streetName)
                .IsRequired()
                .HasColumnName("streetName")
                .HasAnnotation("DisplayName", "Street name");

            builder.Property(e => e.houseNumber)
                .IsRequired()
                .HasColumnName("houseNumber")
                .HasAnnotation("DisplayName", "House number");

            builder.Property(e => e.flatNumber)
                //.IsRequired()
                .HasColumnName("flatNumber")
                .HasAnnotation("DisplayName", "Flat number");

            builder.Property(e => e.postcode)
                .IsRequired()
                .HasColumnName("postCode")
                .HasAnnotation("DisplayName", "Postcode");

            builder.Property(e => e.city)
                .IsRequired();
        }
    }
}
