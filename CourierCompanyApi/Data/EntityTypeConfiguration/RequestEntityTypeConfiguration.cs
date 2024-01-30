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
    public class RequestEntityTypeConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasKey(e => e.Id);

			builder.Property(e => e.GUID)
	            .IsRequired();

			builder.Property(e => e.offerId)
                .IsRequired();

            builder.HasOne(e => e.offer)
                .WithMany()
                .HasForeignKey(e => e.offerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.requestStatus)
                .IsRequired();

            builder.Property(e => e.decisionDeadline)
                .IsRequired();

            builder.Property(e => e.personalDataId)
                .IsRequired();

            builder.HasOne(e => e.personalData)
                .WithMany()
                .HasForeignKey(e => e.personalDataId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.rejectionReason);
        }
    }
}
