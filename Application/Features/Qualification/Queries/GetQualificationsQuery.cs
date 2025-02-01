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
    public class GetQualificationsQuery : IRequest<Result<ICollection<QualificationDto>>>
    {
    }

    public class GetQualificationsQueryHandler(IUnitOfWork unitOfWork): IRequestHandler<GetQualificationsQuery, Result<ICollection<QualificationDto>>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task<Result<ICollection<QualificationDto>>> Handle(GetQualificationsQuery request, CancellationToken cancellationToken)
        {
            return Result<ICollection<QualificationDto>>.Ok(await unitOfWork.QualificationRepository.GetQualifications());
        }
    }
}
