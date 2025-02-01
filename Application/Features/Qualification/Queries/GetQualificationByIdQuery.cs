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

namespace Application.Features.Qualification.Queries
{
    public class GetQualificationByIdQuery : IRequest<Result<QualificationDto>>
    {
        public required int Id { get; set; }
    }

    public class GetQualificationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetQualificationByIdQuery, Result<QualificationDto>>
    {

        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;

        public async Task<Result<QualificationDto>> Handle(GetQualificationByIdQuery request, CancellationToken cancellationToken)
        {
            var qualification = await unitOfWork.QualificationRepository.GetQualification(request.Id);
            if (qualification is null)
            {
                return Result<QualificationDto>.NotFound($"La calificación con id {request.Id} no existe");
            }

            return Result<QualificationDto>.Ok(qualification);
        }
    }

}
