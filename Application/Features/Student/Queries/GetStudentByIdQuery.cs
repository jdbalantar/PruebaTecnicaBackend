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
    public class GetStudentByIdQuery : IRequest<Result<StudentDto>>
    {
        public required int Id { get; set; }
    }

    public class GetStudentByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetStudentByIdQuery, Result<StudentDto>>
    {

        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;
        public async Task<Result<StudentDto>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await unitOfWork.StudentRepository.GetByIdAsync(request.Id);
            if (student == null)
            {
                return Result<StudentDto>.NotFound($"No se encontró ningún estudiante con el id {request.Id}");
            }

            return Result<StudentDto>.Ok(student);
        }
    }
}
