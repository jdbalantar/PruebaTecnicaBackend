using Application.DTOs.Teachers;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Infrastructure.Repositories
{
    public interface ITeacherRepository : IBaseRepository<Teacher>
    {
        Task<int> GetUserId(int teacherId);
        Task<ICollection<TeacherDto>?> GetTeachers();
        Task<TeacherDto?> GetTeacher(int id);
    }
}
