using Application.DTOs.Courses;
using Application.Features.Course.Commands;
using Application.Features.Course.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    [AllowAnonymous]
    public class CoursesController : BaseApiController<AuthController>
    {
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var result = await Mediator.Send(new GetCoursesQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var result = await Mediator.Send(new GetCourseByIdQuery { Id = id });
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto Course)
        {
            var result = await Mediator.Send(new CreateCourseCommand { Data = Course });
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] UpdateCourseDto Course)
        {
            var result = await Mediator.Send(new UpdateCourseCommand { Id = id, Data = Course });
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var result = await Mediator.Send(new DeleteCourseCommand { Id = id });
            return StatusCode(result.StatusCode, result);
        }
    }
}
