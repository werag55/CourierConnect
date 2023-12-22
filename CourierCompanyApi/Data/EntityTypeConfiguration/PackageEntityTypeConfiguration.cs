using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourierCompanyApi.Models;

namespace CourierConnect.DataAccess.Data.EntityTypeConfiguration
{
    public class PackageEntityTypeConfiguration : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.width)
                .IsRequired()
                .HasColumnName("width")
                .HasAnnotation("DisplayName", "Package width");

            builder.Property(e => e.height)
                .IsRequired()
                .HasColumnName("height")
                .HasAnnotation("DisplayName", "Package height");

            builder.Property(e => e.length)
                .IsRequired()
                .HasColumnName("length")
                .HasAnnotation("DisplayName", "Package length");

            builder.Property(e => e.dimensionsUnit)
                .IsRequired()
                .HasColumnName("dimensionsUnit")
                .HasAnnotation("DisplayName", "Unit of package dimensions");

            builder.Property(e => e.weight)
                .IsRequired()
                .HasColumnName("weight")
                .HasAnnotation("DisplayName", "Package weight");

            builder.Property(e => e.weightUnit)
                .IsRequired()
                .HasColumnName("weightUnit")
                .HasAnnotation("DisplayName", "Unit of package weight");
        }
    }
}
