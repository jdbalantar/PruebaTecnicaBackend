using Application.Interfaces.Infrastructure.Repositories;
using Infrastructure.DbContext;

namespace Infrastructure.Repositories
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private bool disposed;

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public Task Rollback()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                _dbContext.Dispose();

            disposed = true;
        }


        #region Repositories


        private StudentRepository? _StudentRepository;
        public IStudentRepository StudentRepository
        {
            get
            {
                _StudentRepository ??= new StudentRepository(_dbContext);
                return _StudentRepository;
            }
        }

        private TeacherRepository? _TeacherRepository;
        public ITeacherRepository TeacherRepository
        {
            get
            {
                _TeacherRepository ??= new TeacherRepository(_dbContext);
                return _TeacherRepository;
            }
        }

        private CourseRepository? _CourseRepository;
        public ICourseRepository CourseRepository
        {
            get
            {
                _CourseRepository ??= new CourseRepository(_dbContext);
                return _CourseRepository;
            }
        }

        private QualificationRepository? _QualificationRepository;
        public IQualificationRepository QualificationRepository
        {
            get
            {
                _QualificationRepository ??= new QualificationRepository(_dbContext);
                return _QualificationRepository;
            }
        }

        #endregion
    }
}
