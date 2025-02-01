using Application.DTOs.Teachers;
using Application.Features.Teacher.Commands;
using Application.Features.Teacher.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    public class TeachersController : BaseApiController<AuthController>
    {

        [HttpGet]
        public async Task<IActionResult> GetTeachers()
        {
            var result = await Mediator.Send(new GetTeachersQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(int id)
        {
            var result = await Mediator.Send(new GetTeacherByIdQuery { Id = id });
            return StatusCode(result.StatusCode, result);
        }

        
        [HttpPost]
        public async Task<IActionResult> CreateTeacher([FromBody] CreateTeacherDto teacher)
        {
            var result = await Mediator.Send(new CreateTeacherCommand { Data = teacher });
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacher(int id, [FromBody] UpdateTeacherDto teacher)
        {
            var result = await Mediator.Send(new UpdateTeacherCommand { Id = id, Data = teacher });
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            var result = await Mediator.Send(new DeleteTeacherCommand { Id = id });
            return StatusCode(result.StatusCode, result);
        }


    }
}
