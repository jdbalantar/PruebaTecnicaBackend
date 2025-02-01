using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityConfiguration
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.ToTable("Teachers");
            builder.Property(x => x.UserId).IsRequired();

            builder.HasMany(x => x.Courses)
                .WithOne(x => x.Teacher)
                .HasForeignKey(x => x.TeacherId);

            builder.HasData(
                new Teacher
                {
                    Id = 1,
                    UserId = 1
                }
            );
        }
    }
}
