using Application.DTOs.Qualifications;
using Domain.Entities;

namespace Application.Interfaces.Infrastructure.Repositories
{
    public interface IQualificationRepository : IBaseRepository<Qualification>
    {
        Task<QualificationDto?> GetQualification(int id);
        Task<ICollection<QualificationDto>?> GetQualifications();
    }
}
