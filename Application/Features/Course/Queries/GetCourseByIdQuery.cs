using Application.DTOs;
using Application.DTOs.Courses;
using Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Course.Queries
{
    public class GetCourseByIdQuery : IRequest<Result<CourseDto>>
    {
        public required int Id { get; set; }
    }

    public class GetCourseByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCourseByIdQuery, Result<CourseDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;


        public async Task<Result<CourseDto>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await unitOfWork.CourseRepository.GetCourse(request.Id);
            if (course is null)
            {
                return Result<CourseDto>.NotFound($"No se encontró ningún curso con el id {request.Id}");
            }

            return Result<CourseDto>.Ok(course);
        }
    }
}
