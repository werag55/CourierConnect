using CourierConnect.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.DataAccess.Data.EntityTypeConfiguration
{
    public class DeliveryEntityTypeConfiguration : IEntityTypeConfiguration<Delivery>
    {
        public void Configure(EntityTypeBuilder<Delivery> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.companyId)
                .IsRequired();

            builder.Property(e => e.companyDeliveryId)
                .IsRequired();

            //builder.Property(e => e.courierName)
            //    .IsRequired();

            //builder.Property(e => e.courierSurname)
            //    .IsRequired();

            builder.Property(e => e.requestId)
                .IsRequired();

            builder.HasOne(e => e.request)
                .WithMany()
                .HasForeignKey(e => e.requestId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Property(e => e.cancelationDeadline)
            //    .IsRequired();

            //builder.Property(e => e.pickUpDate);

            //builder.Property(e => e.deliveryDate);

            //builder.Property(e => e.deliveryStatus)
            //    .IsRequired();

            //builder.Property(e => e.reason);
        }
    }
}
