using Application.DTOs;
using Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Course.Commands
{
    public class DeleteCourseCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }

    public class DeleteCourseCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCourseCommand, Result<bool>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        public async Task<Result<bool>> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            bool hasStudents = await unitOfWork.CourseRepository.Exists(x => x.Id == request.Id && x.Students!.Any(), cancellationToken);
            if (hasStudents)
            {
                return Result<bool>.Conflict($"No se puede eliminar el curso con id {request.Id} porque tiene estudiantes inscritos");
            }

            var course = await unitOfWork.CourseRepository.GetByIdAsync(request.Id, cancellationToken);
            if (course is null)
            {
                return Result<bool>.NotFound($"No se encontró ningún curso con el id {request.Id}");
            }

            unitOfWork.CourseRepository.Delete(course);
            bool success = await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<bool>.Ok(success);
        }
    }
}
