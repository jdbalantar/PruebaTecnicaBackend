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
    public class CreateStudentCommand : IRequest<Result<StudentDto>>
    {
        public required CreateStudentDto Data { get; set; }
    }

    public class CreateStudentCommandHandler(IUnitOfWork unitOfWork, UserManager<User> userManager) : IRequestHandler<CreateStudentCommand, Result<StudentDto>>
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly UserManager<User> userManager = userManager;

        public async Task<Result<StudentDto>> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                bool courseExists = await unitOfWork.CourseRepository.Exists(x => x.Id == request.Data.CourseId, cancellationToken);
                if (!courseExists)
                {
                    return Result<StudentDto>.Error("No existe ningún curso con el id " + request.Data.CourseId);
                }

                var user = new User
                {
                    FirstName = request.Data.FirstName,
                    LastName = request.Data.LastName,
                    Identification = request.Data.Identification,
                    UserName = request.Data.Identification,
                    Email = request.Data.Email
                };

                var result = await userManager.CreateAsync(user);

                if (!result.Succeeded)
                {
                    return Result<StudentDto>.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                var student = new Domain.Entities.Student()
                {
                    UserId = user.Id,
                    CourseId = request.Data.CourseId
                };

                await unitOfWork.StudentRepository.AddAsync(student, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                var studentDto = await unitOfWork.StudentRepository.GetByIdAsync(student.Id);
                transaction.Complete();

                return Result<StudentDto>.Ok(studentDto);

            }

        }
    }
}
