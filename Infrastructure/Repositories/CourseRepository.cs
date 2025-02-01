using Application.DTOs.Courses;
using Application.Interfaces.Infrastructure.Repositories;
using Domain.Entities;
using Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CourseRepository(ApplicationDbContext context) : BaseRepository<Course>(context), ICourseRepository
    {
        private readonly ApplicationDbContext context = context;

        public async Task<CourseDto?> GetCourse(int id)
        {
            return await context.Courses
                .Where(x => x.Id == id)
                .Include(x => x.Students)?
                    .ThenInclude(s => s.User)
                .Include(x => x.Teacher)
                    .ThenInclude(t => t.User)
                .Select(x => new CourseDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Teacher = x.Teacher != null && x.Teacher.User != null ? x.Teacher.User.FullName : string.Empty,
                    Students = x.Students != null
                        ? x.Students.Where(s => s.User != null).Select(s => s.User!.FullName).ToList()
                        : new List<string>()
                }).FirstOrDefaultAsync();
        }

        public async Task<ICollection<CourseDto>?> GetCourses()
        {
            return await context.Courses
                .Include(x => x.Students)?
                    .ThenInclude(s => s.User)
                .Include(x => x.Teacher)
                    .ThenInclude(t => t.User)
                .Select(x => new CourseDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Teacher = x.Teacher != null && x.Teacher.User != null ? x.Teacher.User.FullName : string.Empty,
                    Students = x.Students != null
                        ? x.Students.Where(s => s.User != null).Select(s => s.User!.FullName).ToList()
                        : new List<string>()
                }).ToListAsync();
        }

    }
}
