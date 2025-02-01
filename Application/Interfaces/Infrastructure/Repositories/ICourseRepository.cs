using Application.DTOs.Courses;
using Domain.Entities;

namespace Application.Interfaces.Infrastructure.Repositories
{
    public interface ICourseRepository : IBaseRepository<Course>
    {
        Task<CourseDto?> GetCourse(int id);
        Task<ICollection<CourseDto>?> GetCourses();
    }
}
