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
    public class GetTeacherByIdQuery : IRequest<Result<TeacherDto>>
    {
        public required int Id { get; set; }
    }

    public class GetTeacherByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetTeacherByIdQuery, Result<TeacherDto>>
    {

        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result<TeacherDto>> Handle(GetTeacherByIdQuery request, CancellationToken cancellationToken)
        {
            var teacher = await unitOfWork.TeacherRepository.GetTeacher(request.Id);
            if (teacher is null)
            {
                return Result<TeacherDto>.NotFound($"No se encontró ningún profesor con el id {request.Id}");
            }
            return Result<TeacherDto>.Ok(teacher);
        }
    }
}
