using Application.DTOs;
using Application.DTOs.Teachers;
using Application.Interfaces.Infrastructure.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Features.Teacher.Commands
{
    public class UpdateTeacherCommand : IRequest<Result<TeacherDto>>
    {
        public required int Id { get; set; }
        public required UpdateTeacherDto Data { get; set; }
    }

    public class UpdateTeacherCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager) : IRequestHandler<UpdateTeacherCommand, Result<TeacherDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly UserManager<User> userManager = userManager;

        public async Task<Result<TeacherDto>> Handle(UpdateTeacherCommand request, CancellationToken cancellationToken)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                bool exists = await unitOfWork.TeacherRepository.Exists(x => x.Id == request.Id, cancellationToken);
                if (!exists)
                {
                    return Result<TeacherDto>.NotFound($"No se encontró ningún profesor con el id {request.Id}");
                }

                int userId = await unitOfWork.TeacherRepository.GetUserId(request.Id);

                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user is null)
                {
                    return Result<TeacherDto>.NotFound($"No se encontró ningún usuario con el id {request.Id}");
                }

                user.Identification = request.Data.Identification;
                user.FirstName = request.Data.FirstName;
                user.LastName = request.Data.LastName;
                user.Email = request.Data.Email;

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return Result<TeacherDto>.Error(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                }


                var result = await unitOfWork.TeacherRepository.GetTeacher(request.Id);
                transaction.Complete();
                return Result<TeacherDto>.Ok(result);
            }
        }
    }
}
