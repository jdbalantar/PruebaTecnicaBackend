using Application.DTOs;
using Application.DTOs.Courses;
using Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Course.Commands
{
    public class UpdateCourseCommand : IRequest<Result<CourseDto>>
    {
        public required int Id { get; set; }
        public required UpdateCourseDto Data { get; set; }
    }

    public class UpdateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateCourseCommand, Result<CourseDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<Result<CourseDto>> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await unitOfWork.CourseRepository.GetByIdAsync(request.Id);
            if (course is null)
            {
                return Result<CourseDto>.NotFound($"No se encontró ningún curso con el id {request.Id}");
            }

            bool existe = await unitOfWork.TeacherRepository.Exists(x => x.Id == request.Data.TeacherId, cancellationToken);
            if (!existe)
            {
                return Result<CourseDto>.NotFound("No existe el docente con el id " + request.Data.TeacherId);
            }

            mapper.Map(request.Data, course);
            unitOfWork.CourseRepository.Update(course);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var result = await unitOfWork.CourseRepository.GetCourse(course.Id);
            return Result<CourseDto>.Ok(result);
        }
    }

}
