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
    public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.companyId)
                .IsRequired();

            builder.Property(e => e.Name)
                .IsRequired();
        }
    }
}
