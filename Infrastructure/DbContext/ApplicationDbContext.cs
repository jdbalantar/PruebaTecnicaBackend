using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces.Shared;
using Domain.Entities.Base;
using Application.Enums;
using Application.DTOs.Logs;
using Application.Interfaces.Infrastructure;
using Infrastructure.EntityConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DbContext
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IAuthenticatedUserService authenticatedUser, IHttpContextAccessor httpContextAccessor) : IdentityDbContext<User, IdentityRole<int>, int>(options), IApplicationDbContext
    {
        private readonly IAuthenticatedUserService _authenticatedUser = authenticatedUser;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public DbSet<Audit> AuditLogs { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }



        public IDbConnection Connection => Database.GetDbConnection();
        public bool HasChanges => ChangeTracker.HasChanges();
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {

            foreach (var entry in ChangeTracker.Entries<AuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId ?? "SYSTEM";
                        entry.Property(nameof(entry.Entity.LastModifiedOn)).IsModified = false;
                        entry.Property(nameof(entry.Entity.LastModifiedBy)).IsModified = false;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId ?? "SYSTEM";
                        entry.Property(nameof(entry.Entity.CreatedOn)).IsModified = false;
                        entry.Property(nameof(entry.Entity.CreatedBy)).IsModified = false;
                        break;
                }
            }

            List<AuditEntry> auditEntries = OnBeforeSaveChanges(_authenticatedUser.UserId ?? "SYSTEM");
            int result = await base.SaveChangesAsync(cancellationToken);
            await OnAfterSaveChanges(auditEntries);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new IdentityUserConfiguration());
            builder.ApplyConfiguration(new IdentityRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserClaimConfiguration());
            builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            builder.ApplyConfiguration(new IdentityRoleClaimConfiguration());
            builder.ApplyConfiguration(new IdentityUserTokenConfiguration());
            builder.ApplyConfiguration(new TeacherConfiguration());
            builder.ApplyConfiguration(new StudentConfiguration());
            DecimalPropertyConfiguration.Configure(builder);
            AuditableEntityConfiguration.Configure(builder);

            SeedData(builder);
        }


        private List<AuditEntry> OnBeforeSaveChanges(string userId)
        {
            ChangeTracker.DetectChanges();
            List<AuditEntry> auditEntries = [];
            var httpContext = _httpContextAccessor.HttpContext;
            string controllerName = httpContext?.GetRouteValue("controller")?.ToString() ?? "Controlador desconocido";
            string actionName = httpContext?.GetRouteValue("action")?.ToString() ?? "Acción desconocida";



            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                var auditEntry = new AuditEntry(entry) { TableName = entry.Entity.GetType().Name, UserId = userId, ActionName = actionName, ControllerName = controllerName };
                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties.Where(p => !p.IsTemporary))
                {
                    string propertyName = property.Metadata.Name;

                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue!;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue!;
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue!;
                            break;

                        case EntityState.Modified when property.IsModified:
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue!;
                            auditEntry.NewValues[propertyName] = property.CurrentValue!;
                            break;
                    }
                }

            }

            foreach (var auditEntry in auditEntries.Where(ae => !ae.HasTemporaryProperties))
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }

            return auditEntries.Where(ae => ae.HasTemporaryProperties).ToList();
        }

        private async Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0) return;

            foreach (var auditEntry in auditEntries)
            {
                auditEntry.TemporaryProperties
                    .Where(prop => prop is not null)
                    .ToList()
                    .ForEach(prop =>
                    {
                        var targetDictionary = prop.Metadata.IsPrimaryKey() ? auditEntry.KeyValues : auditEntry.NewValues;
                        targetDictionary[prop.Metadata.Name] = prop.CurrentValue!;
                    });

                AuditLogs.Add(auditEntry.ToAudit());
            }

            await SaveChangesAsync();
        }

        private void SeedData(ModelBuilder builder)
        {
            // 🔹 Usuarios (Docentes y Estudiantes separados)
            var teacherUser1 = new User { Id = 1, FirstName = "Carlos", LastName = "García", Identification = "100000001", UserName = "carlosg", EmailConfirmed = true };
            var teacherUser2 = new User { Id = 2, FirstName = "Lucía", LastName = "Fernández", Identification = "100000002", UserName = "luciaf", EmailConfirmed = true };

            var studentUser1 = new User { Id = 3, FirstName = "Miguel", LastName = "Torres", Identification = "200000001", UserName = "miguelt", EmailConfirmed = true };
            var studentUser2 = new User { Id = 4, FirstName = "Ana", LastName = "Martínez", Identification = "200000002", UserName = "anam", EmailConfirmed = true };
            var studentUser3 = new User { Id = 5, FirstName = "Pedro", LastName = "Ramírez", Identification = "200000003", UserName = "pedror", EmailConfirmed = true };

            builder.Entity<User>().HasData(teacherUser1, teacherUser2, studentUser1, studentUser2, studentUser3);

            // 🔹 Profesores (Evitar IDs duplicados)
            var teacher1 = new Teacher { Id = 10, UserId = teacherUser1.Id }; // 🔹 Cambié el Id a 10
            var teacher2 = new Teacher { Id = 11, UserId = teacherUser2.Id }; // 🔹 Cambié el Id a 11

            builder.Entity<Teacher>().HasData(teacher1, teacher2);

            // 🔹 Cursos (Con Ids únicos)
            var course1 = new Course { Id = 20, Name = "Matemáticas", Description = "Curso de matemáticas avanzadas", TeacherId = teacher1.Id };
            var course2 = new Course { Id = 21, Name = "Historia", Description = "Historia mundial y civilizaciones", TeacherId = teacher2.Id };
            var course3 = new Course { Id = 22, Name = "Ciencias", Description = "Física y química básica", TeacherId = teacher1.Id };

            builder.Entity<Course>().HasData(course1, course2, course3);

            // 🔹 Estudiantes (Ids únicos)
            var student1 = new Student { Id = 30, UserId = 3, CourseId = course1.Id };
            var student2 = new Student { Id = 31, UserId = 4, CourseId = course2.Id };
            var student3 = new Student { Id = 32, UserId = 5, CourseId = course3.Id };

            builder.Entity<Student>().HasData(student1, student2, student3);

            // 🔹 Calificaciones (Ids únicos)
            var qualification1 = new Qualification { Id = 40, StudentId = student1.Id, Score = 9.2m };
            var qualification2 = new Qualification { Id = 41, StudentId = student2.Id, Score = 8.5m };
            var qualification3 = new Qualification { Id = 42, StudentId = student3.Id, Score = 7.8m };

            builder.Entity<Qualification>().HasData(qualification1, qualification2, qualification3);
        }


    }
}
