using Application.DTOs;
using Application.DTOs.Students;
using Application.DTOs.Teachers;
using Application.Interfaces.Infrastructure.Repositories;
using Application.Transversal;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Features.Teacher.Commands
{
    public class CreateTeacherCommand : IRequest<Result<TeacherDto>>
    {
        public required CreateTeacherDto Data { get; set; }
    }

    public class CreateTeacherCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager) : IRequestHandler<CreateTeacherCommand, Result<TeacherDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly UserManager<User> userManager = userManager;

        public async Task<Result<TeacherDto>> Handle(CreateTeacherCommand request, CancellationToken cancellationToken)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var user = new User
                {
                    FirstName = request.Data.FirstName,
                    LastName = request.Data.LastName,
                    Identification = request.Data.Identification,
                    UserName = request.Data.Identification,
                    Email = request.Data.Email
                };

                var createResult = await userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    return Result<TeacherDto>.BadRequest("No se pudo crear el docente", createResult.GetErrorResult());
                }

                var teacher = new Domain.Entities.Teacher()
                {
                    UserId = user.Id
                };

                await unitOfWork.TeacherRepository.AddAsync(teacher, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                var result = await unitOfWork.TeacherRepository.GetTeacher(teacher.Id);
                transaction.Complete();

                return Result<TeacherDto>.Ok("Profesor creado con éxito", result);

            }
        }
    }
}
