using Application.DTOs.Students;
using Domain.Entities;

namespace Application.Interfaces.Infrastructure.Repositories
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        Task<int> GetUserId(int studentId);
        Task<StudentDto?> GetByIdAsync(int id);
        Task<ICollection<StudentDto>?> GetStudents();
    }
}
