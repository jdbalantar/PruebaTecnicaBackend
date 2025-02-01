using Application.DTOs.Teachers;
using Application.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TeacherRepository(ApplicationDbContext context) : BaseRepository<Teacher>(context), ITeacherRepository
    {
        private readonly ApplicationDbContext context = context;

        public async Task<int> GetUserId(int teacherId) => await context.Teachers.Where(t => t.Id == teacherId).Select(t => t.UserId).FirstOrDefaultAsync();

        public async Task<TeacherDto?> GetTeacher(int id)
        {
            return await context.Teachers
                .Include(x => x.User)
                .Select(x => new TeacherDto()
                {
                    Id = x.Id,
                    FullName = x.User!.FullName,
                    Identification = x.User.Identification
                })
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<TeacherDto>?> GetTeachers()
        {
            return await context.Teachers
                .Include(x => x.User)
                .Select(x => new TeacherDto()
                {
                    Id = x.Id,
                    FullName = x.User!.FullName,
                    Identification = x.User.Identification
                })
                .ToListAsync();
        }

    }

}
