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
            builder.Property(e => e.DeliveryDate)
                .IsRequired()
                .HasColumnName("DeliveryDate")
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

            builder.Property(e => e.descAddressID)
                .IsRequired();

            builder.HasOne(e => e.descAddress)
                .WithMany()
                .HasForeignKey(e => e.descAddressID)
                .HasConstraintName("addressId");
        }
    }
}
