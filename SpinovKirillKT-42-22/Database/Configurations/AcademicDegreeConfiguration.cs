using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SpinovKirillKT_42_22.Models;

namespace SpinovKirillKT_42_22.Database.Configurations
{
    public class AcademicDegreeConfiguration : IEntityTypeConfiguration<AcademicDegree>
    {
        private const string TableName = "AcademicDegrees";
        public void Configure(EntityTypeBuilder<AcademicDegree> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired();
        }
    }
}
