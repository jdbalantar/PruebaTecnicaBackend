using Application.DTOs.Courses;
using Application.DTOs.Qualifications;
using Application.DTOs.Students;
using Application.DTOs.Teachers;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Course, CourseDto>().ReverseMap().ForMember(x => x.Students, y => y.Ignore());
            CreateMap<Course, CreateCourseDto>().ReverseMap();
            CreateMap<Course, UpdateCourseDto>().ReverseMap();

            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, CreateStudentDto>().ReverseMap();
            CreateMap<Student, UpdateStudentDto>().ReverseMap();

            CreateMap<Qualification, QualificationDto>().ReverseMap();
            CreateMap<Qualification, CreateQualificationDto>().ReverseMap();
            CreateMap<Qualification, UpdateQualificationDto>().ReverseMap();

            CreateMap<Teacher, TeacherDto>().ReverseMap();
            CreateMap<Teacher, CreateTeacherDto>().ReverseMap();
            CreateMap<Teacher, UpdateTeacherDto>().ReverseMap();
        }
    }
}
