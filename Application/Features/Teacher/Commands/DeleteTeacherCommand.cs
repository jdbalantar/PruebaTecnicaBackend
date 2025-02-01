using Application.DTOs;
using Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Teacher.Commands
{
    public class DeleteTeacherCommand : IRequest<Result<bool?>>
    {
        public required int Id { get; set; }
    }

    public class DeleteTeacherCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteTeacherCommand, Result<bool?>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result<bool?>> Handle(DeleteTeacherCommand request, CancellationToken cancellationToken)
        {
            var teacher = await unitOfWork.TeacherRepository.GetByIdAsync(request.Id, cancellationToken);
            if (teacher is null)
            {
                return Result<bool?>.NotFound($"No se encontró ningún profesor con el id {request.Id}");
            }

            unitOfWork.TeacherRepository.Delete(teacher);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<bool?>.Ok("Profesor eliminado con éxito");
        }
    }
}
