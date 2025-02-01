using Application.DTOs.Qualifications;
using Application.Features.Qualification.Commands;
using Application.Features.Qualification.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.v1
{
    public class QualificationsController : BaseApiController<AuthController>
    {
        [HttpGet]
        public async Task<IActionResult> GetQualifications()
        {
            var result = await Mediator.Send(new GetQualificationsQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQualificationById(int id)
        {
            var result = await Mediator.Send(new GetQualificationByIdQuery { Id = id });
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateQualification([FromBody] CreateQualificationDto Qualification)
        {
            var result = await Mediator.Send(new CreateQualificationCommand { Data = Qualification });
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQualification(int id, [FromBody] UpdateQualificationDto Qualification)
        {
            var result = await Mediator.Send(new UpdateQualificationCommand { Id = id, Data = Qualification });
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQualification(int id)
        {
            var result = await Mediator.Send(new DeleteQualificationCommand { Id = id });
            return StatusCode(result.StatusCode, result);
        }
    }
}
