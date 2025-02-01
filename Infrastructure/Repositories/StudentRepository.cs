using Application.DTOs.Students;
using Application.DTOs.Teachers;
using Application.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class StudentRepository(ApplicationDbContext context) : BaseRepository<Student>(context), IStudentRepository
    {
        private readonly ApplicationDbContext context = context;
        public async Task<int> GetUserId(int studentId) => await context.Students.Where(t => t.Id == studentId).Select(t => t.UserId).FirstOrDefaultAsync();

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            return await context.Students
                .Include(x => x.User)
                .Include(x => x.Course)
                .Select(x => new StudentDto()
                {
                    Id = x.Id,
                    FullName = x.User!.FullName,
                    Identification = x.User.Identification,
                    Course = x.Course!.Name
                })
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ICollection<StudentDto>?> GetStudents()
        {
            return await context.Students
                .Include(x => x.User)
                .Include(x => x.Course)
                .Select(x => new StudentDto()
                {
                    Id = x.Id,
                    FullName = x.User!.FullName,
                    Identification = x.User.Identification,
                    Course = x.Course!.Name
                })
                .ToListAsync();
        }
    }
}
