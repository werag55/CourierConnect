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
    public class OfferEntityTypeConfiguration : IEntityTypeConfiguration<Offer>
    {
        public void Configure(EntityTypeBuilder<Offer> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.companyOfferId)
                .IsRequired();

            builder.Property(e => e.companyId)
                .IsRequired();

            builder.Property(e => e.inquiryId)
                .IsRequired();

            builder.HasOne(e => e.inquiry)
                .WithMany()
                .HasForeignKey(e => e.inquiryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.creationDate)
                .IsRequired();

            builder.Property(e => e.updatedDate)
                .IsRequired();

            builder.Property(e => e.expirationDate)
                .IsRequired();

            builder.Property(e => e.status)
                .IsRequired();

            builder.Property(e => e.price)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(e => e.taxes)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(e => e.fees)
                .IsRequired()
                .HasColumnType("money");

            builder.Property(e => e.currency)
                .IsRequired();
        }
    }
}
