using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityConfiguration
{
    public static class AuditableEntityConfiguration
    {
        public static void Configure(ModelBuilder builder)
        {
            builder.Model.GetEntityTypes()
                .Where(entityType => typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                .ToList()
                .ForEach(entityType =>
                {
                    var entity = builder.Entity(entityType.ClrType);
                    entity.HasKey("Id");
                    entity.Property("Id").IsRequired(true).ValueGeneratedOnAdd();
                    entity.Property<string>("CreatedBy").IsRequired().HasMaxLength(50);
                    entity.Property<DateTime>("CreatedOn").IsRequired();
                    entity.Property<string>("LastModifiedBy").HasMaxLength(50);
                    entity.Property<DateTime?>("LastModifiedOn");
                });

        }
    }
}
