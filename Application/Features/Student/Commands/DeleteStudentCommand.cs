using Application.DTOs;
using Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Student.Commands
{
    public class DeleteStudentCommand : IRequest<Result<bool?>>
    {
        public required int Id { get; set; }
    }

    public class DeleteStudentCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteStudentCommand, Result<bool?>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result<bool?>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await unitOfWork.StudentRepository.GetByIdAsync(request.Id, cancellationToken);
            if (student is null)
            {
                return Result<bool?>.NotFound($"No se encontró ningún estudiante con el id {request.Id}");
            }

            unitOfWork.StudentRepository.Delete(student);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<bool?>.Ok();
        }
    }
}
