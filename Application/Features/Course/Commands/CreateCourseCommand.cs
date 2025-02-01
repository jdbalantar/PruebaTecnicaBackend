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
    public class CreateCourseCommand : IRequest<Result<CourseDto>>
    {
        public required CreateCourseDto Data { get; set; }
    }

    public class CreateCourseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCourseCommand, Result<CourseDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<Result<CourseDto>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            bool existe = await unitOfWork.TeacherRepository.Exists(x => x.Id == request.Data.TeacherId, cancellationToken);
            if (!existe)
            {
                return Result<CourseDto>.NotFound("No existe el docente con el id " + request.Data.TeacherId);
            }
            var course = mapper.Map<Domain.Entities.Course>(request.Data);
            await unitOfWork.CourseRepository.AddAsync(course, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var result = await unitOfWork.CourseRepository.GetCourse(course.Id);
            return Result<CourseDto>.Ok(result);
        }
    }
}
