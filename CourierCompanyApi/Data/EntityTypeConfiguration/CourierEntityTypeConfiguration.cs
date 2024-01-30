using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using CourierCompanyApi.Models;

namespace CourierCompanyApi.Data.EntityTypeConfiguration
{
    public class CourierEntityTypeConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.name)
                .IsRequired();

            builder.Property(e => e.surname)
                .IsRequired();

            builder.Property(e => e.userName)
                .IsRequired();
        }
    }
}
