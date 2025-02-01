using Application.DTOs.Students;
using Application.Features.Student.Commands;
using Application.Features.Student.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [AllowAnonymous]
    public class StudentController : BaseApiController<AuthController>
    {
        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            var result = await Mediator.Send(new GetStudentsQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var result = await Mediator.Send(new GetStudentByIdQuery { Id = id });
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto student)
        {
            var result = await Mediator.Send(new CreateStudentCommand { Data = student });
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] UpdateStudentDto student)
        {
            var result = await Mediator.Send(new UpdateStudentCommand { Id = id, Data = student });
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var result = await Mediator.Send(new DeleteStudentCommand { Id = id });
            return StatusCode(result.StatusCode, result);
        }
    }
}
