using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using CourierConnect.Models;

namespace CourierConnect.DataAccess.Data.EntityTypeConfiguration
{
    public class InquiryEntityTypeConfiguration : IEntityTypeConfiguration<Inquiry>
    {
        public void Configure(EntityTypeBuilder<Inquiry> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.pickupDate)
                .IsRequired()
                .HasColumnName("pickupDate")
                .HasAnnotation("DisplayName", "Pickup date");

            builder.Property(e => e.deliveryDate)
                .IsRequired()
                .HasColumnName("deliveryDate")
                .HasAnnotation("DisplayName", "Delivery date");

            builder.Property(e => e.isPriority)
                .IsRequired()
                .HasColumnName("isPriority")
                .HasAnnotation("DisplayName", "Priority");

            builder.Property(e => e.weekendDelivery)
                .IsRequired()
                .HasColumnName("weekendDelivery")
                .HasAnnotation("DisplayName", "Delivery at weekend");

            builder.Property(e => e.isCompany)
                .IsRequired()
                .HasColumnName("isCompany")
                .HasAnnotation("DisplayName", "Company");

            builder.Property(e => e.sourceAddressId)
                .IsRequired();

            builder.HasOne(e => e.sourceAddress)
                .WithMany()
                .HasForeignKey(e => e.sourceAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.destinationAddressId)
                .IsRequired();

            builder.HasOne(e => e.destinationAddress)
                .WithMany()
                .HasForeignKey(e => e.destinationAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.packageId)
                .IsRequired();

            builder.HasOne(e => e.package)
                .WithMany()
                .HasForeignKey(e => e.packageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
