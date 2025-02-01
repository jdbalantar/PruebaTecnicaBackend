using Application.DTOs;
using Application.DTOs.Teachers;
using Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Teacher.Queries
{
    public class GetTeachersQuery : IRequest<Result<ICollection<TeacherDto>>>
    {
    }

    public class GetTeachersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTeachersQuery, Result<ICollection<TeacherDto>>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result<ICollection<TeacherDto>>> Handle(GetTeachersQuery request, CancellationToken cancellationToken)
        {
            return Result<ICollection<TeacherDto>>.Ok(await unitOfWork.TeacherRepository.GetTeachers());
        }
    }
}
