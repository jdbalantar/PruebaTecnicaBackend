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
    public class UpdateQualificationCommand : IRequest<Result<QualificationDto>>
    {
        public required int Id { get; set; }
        public required UpdateQualificationDto Data { get; set; }
    }

    public class UpdateQualificationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateQualificationCommand, Result<QualificationDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;
        public async Task<Result<QualificationDto>> Handle(UpdateQualificationCommand request, CancellationToken cancellationToken)
        {
            var qualification = await unitOfWork.QualificationRepository.GetByIdAsync(request.Id, cancellationToken);
            if (qualification is null)
            {
                return Result<QualificationDto>.NotFound($"La calificación con id {request.Id} no existe");
            }

            bool studentExists = await unitOfWork.StudentRepository.Exists(x => x.Id == request.Data.StudentId, cancellationToken);
            if (!studentExists)
            {
                return Result<QualificationDto>.NotFound($"No se encontró ningún estudiante con el id {request.Data.StudentId}");
            }

            mapper.Map(request.Data, qualification);
            unitOfWork.QualificationRepository.Update(qualification);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            var result = await unitOfWork.QualificationRepository.GetQualification(qualification.Id);
            return Result<QualificationDto>.Ok(result);

        }
    }
}
