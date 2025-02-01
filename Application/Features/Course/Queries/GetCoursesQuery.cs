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

namespace Application.Features.Course.Queries
{
    public class GetCoursesQuery : IRequest<Result<ICollection<CourseDto>>>
    {
    }

    public class GetCoursesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCoursesQuery, Result<ICollection<CourseDto>>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<Result<ICollection<CourseDto>>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            return Result<ICollection<CourseDto>>.Ok(await unitOfWork.CourseRepository.GetCourses());
        }
    }
}
