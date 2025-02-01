using Application.DTOs;
using Application.DTOs.Students;
using Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Student.Queries
{
    public class GetStudentsQuery : IRequest<Result<ICollection<StudentDto>>>
    {
    }

    public class GetStudentsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetStudentsQuery, Result<ICollection<StudentDto>>>
    {

        private readonly IUnitOfWork unitOfWork = unitOfWork;


        public async Task<Result<ICollection<StudentDto>>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            return Result<ICollection<StudentDto>>.Ok(await unitOfWork.StudentRepository.GetStudents());
        }
    }
}
