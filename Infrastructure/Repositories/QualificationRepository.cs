using Application.DTOs.Qualifications;
using Application.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class QualificationRepository(ApplicationDbContext context) : BaseRepository<Qualification>(context), IQualificationRepository
    {
        private readonly ApplicationDbContext context = context;

        public async Task<QualificationDto?> GetQualification(int id)
        {
            return await context.Qualifications
                .Where(q => q.Id == id)
                .Include(q => q.Student)
                    .ThenInclude(s => s!.User)
                .Select(q => new QualificationDto
                {
                    Id = q.Id,
                    Student = q.Student != null && q.Student.User != null
                        ? q.Student.User.FullName
                        : string.Empty,
                    Score = q.Score
                })
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<QualificationDto>?> GetQualifications()
        {
            return await context.Qualifications
                .Include(q => q.Student)
                    .ThenInclude(s => s!.User)
                .Select(q => new QualificationDto
                {
                    Id = q.Id,
                    Student = q.Student != null && q.Student.User != null
                        ? q.Student.User.FullName
                        : string.Empty,
                    Score = q.Score
                })
                .ToListAsync();
        }

    }
}
