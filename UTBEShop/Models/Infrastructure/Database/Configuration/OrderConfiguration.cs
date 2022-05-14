using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UTBEShop.Models.Entities;

namespace UTBEShop.Models.Infrastructure.Database.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(nameof(Order.DateTimeCreated))
                .HasDefaultValueSql("getdate()");
        }
    }
}
