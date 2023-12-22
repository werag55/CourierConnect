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
    public class PersonalDataEntityTypeConfiguration : IEntityTypeConfiguration<PersonalData>
    {
        public void Configure(EntityTypeBuilder<PersonalData> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.name)
                .IsRequired();

            builder.Property(e => e.surname)
                .IsRequired();

            builder.Property(e => e.companyName);

            builder.Property(e => e.addressId)
                .IsRequired();

            builder.HasOne(e => e.address)
                .WithMany()
                .HasForeignKey(e => e.addressId)
                .IsRequired()
                .HasConstraintName("FK_PersonalData_Address_addressId");

            builder.Property(e => e.email)
                .IsRequired();
        }
    }
}
