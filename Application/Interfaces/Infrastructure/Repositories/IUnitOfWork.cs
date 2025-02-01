namespace Application.Interfaces.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task Rollback();

        IStudentRepository StudentRepository { get; }
        ITeacherRepository TeacherRepository { get; }
        ICourseRepository CourseRepository { get; }
        IQualificationRepository QualificationRepository { get; }
    }
}
