using Application.DTOs;
using Application.DTOs.Students;
using Application.DTOs.Teachers;
using Application.Interfaces.Infrastructure.Repositories;
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

namespace Application.Features.Student.Commands
{
    public class UpdateStudentCommand : IRequest<Result<StudentDto>>
    {
        public int Id { get; set; }
        public required UpdateStudentDto Data { get; set; }
    }

    public class UpdateStudentCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager) : IRequestHandler<UpdateStudentCommand, Result<StudentDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly UserManager<User> userManager = userManager;

        public async Task<Result<StudentDto>> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                bool exists = await unitOfWork.StudentRepository.Exists(x => x.Id == request.Id, cancellationToken);
                if (!exists)
                {
                    return Result<StudentDto>.NotFound($"No se encontró ningún estudiante con el id {request.Id}");
                }

                exists = await unitOfWork.CourseRepository.Exists(x => x.Id == request.Data.CourseId, cancellationToken);
                if (!exists)
                {
                    return Result<StudentDto>.Error("No existe ningún curso con el id " + request.Data.CourseId);
                }

                int userId = await unitOfWork.StudentRepository.GetUserId(request.Id);

                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user is null)
                {
                    return Result<StudentDto>.NotFound($"No se encontró ningún usuario con el id {request.Id}");
                }

                user.Identification = request.Data.Identification;
                user.FirstName = request.Data.FirstName;
                user.LastName = request.Data.LastName;
                user.Email = request.Data.Email;

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return Result<StudentDto>.BadRequest(string.Join(", ", updateResult.Errors.Select(e => e.Description)));
                }

                var studentToUpdate = await unitOfWork.StudentRepository.FindAsync(x => x.Id == request.Id);
                studentToUpdate!.CourseId = request.Data.CourseId;
                unitOfWork.StudentRepository.Update(studentToUpdate);
                var student = await unitOfWork.StudentRepository.GetByIdAsync(request.Id);
                transaction.Complete();

                return Result<StudentDto>.Ok(student);
            }

        }
    }
}
