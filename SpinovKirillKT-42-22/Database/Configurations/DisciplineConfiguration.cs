using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SpinovKirillKT_42_22.Models;

namespace SpinovKirillKT_42_22.Database.Configurations
{
    public class DisciplineConfiguration : IEntityTypeConfiguration<Discipline>
    {
        private const string TableName = "Disciplines";

        public void Configure(EntityTypeBuilder<Discipline> builder)
        {
            builder.ToTable(TableName);

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired();


            builder.HasMany(d => d.Loads)
                .WithOne(l => l.Discipline)
                .HasForeignKey(l => l.DisciplineId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
