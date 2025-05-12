using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SpinovKirillKT_42_22.Models;

namespace SpinovKirillKT_42_22.Database.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        private const string TableName = "Posts";
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired();
        }
    }
}
