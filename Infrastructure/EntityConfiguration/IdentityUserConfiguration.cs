using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityConfiguration
{
    public class IdentityUserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);
            builder.Property(builder => builder.Id)
                    .ValueGeneratedOnAdd();

            builder.Ignore(e => e.FullName);

            builder.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasOne(u => u.Student)
                .WithOne(s => s.User)
                .HasForeignKey<Student>(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Teacher)
                .WithOne(s => s.User)
                .HasForeignKey<Teacher>(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
