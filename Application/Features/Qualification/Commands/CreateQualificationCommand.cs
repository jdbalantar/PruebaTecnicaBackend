using Application.DTOs;
using Application.DTOs.Qualifications;
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
    public class CreateQualificationCommand : IRequest<Result<QualificationDto>>
    {
        public required CreateQualificationDto Data { get; set; }
    }

    public class CreateQualificationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateQualificationCommand, Result<QualificationDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<Result<QualificationDto>> Handle(CreateQualificationCommand request, CancellationToken cancellationToken)
        {
            bool studentExists = await unitOfWork.StudentRepository.Exists(x => x.Id == request.Data.StudentId, cancellationToken);
            if (!studentExists)
            {
                return Result<QualificationDto>.NotFound($"No se encontró ningún estudiante con el id {request.Data.StudentId}");
            }

            var qualification = mapper.Map<Domain.Entities.Qualification>(request.Data);

            await unitOfWork.QualificationRepository.AddAsync(qualification, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var result = await unitOfWork.QualificationRepository.GetQualification(qualification.Id);
            return Result<QualificationDto>.Ok("Calificación creada con éxito", result);
        }
    }
}
