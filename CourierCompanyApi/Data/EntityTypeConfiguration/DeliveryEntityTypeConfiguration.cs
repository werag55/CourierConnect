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
    public class DeliveryEntityTypeConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.courierId)
                .IsRequired();

            builder.HasOne(e => e.courier)
                .WithMany()
                .HasForeignKey(e => e.courierId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.requestId)
                .IsRequired();

            builder.HasOne(e => e.request)
                .WithMany()
                .HasForeignKey(e => e.requestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.pickUpDate)
                .IsRequired();

            builder.Property(e => e.deliveryDate);

            builder.Property(e => e.deliveryStatus)
                .IsRequired();

            builder.Property(e => e.reason);
        }
    }
}
