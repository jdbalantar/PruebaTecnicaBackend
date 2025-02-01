using Application.DTOs;
using Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Qualification.Commands
{
    public class DeleteQualificationCommand : IRequest<Result<bool?>>
    {
        public required int Id { get; set; }
    }

    public class DeleteQualificationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteQualificationCommand, Result<bool?>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        public async Task<Result<bool?>> Handle(DeleteQualificationCommand request, CancellationToken cancellationToken)
        {
            var qualification = await unitOfWork.QualificationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (qualification is null)
            {
                return Result<bool?>.NotFound($"No se encontró ninguna calificación con el id {request.Id}");
            }
            unitOfWork.QualificationRepository.Delete(qualification);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<bool?>.Ok();
        }
    }
}
