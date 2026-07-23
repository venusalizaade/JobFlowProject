using System.Security.Claims;
using JobFlowProject.Business.Dto.JobPost;
using JobFlowProject.Business.Interfaces.JobPost;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;



    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "JobSeeker")]
    public class JobApplicationJobSeekerController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;

        public JobApplicationJobSeekerController(IJobApplicationService jobApplicationService)
        {
            _jobApplicationService = jobApplicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Apply(CreateJobApplicationDto dto)
        {
            var requesterId = GetUserId();

            await _jobApplicationService.ApplyAsync(requesterId, dto);

            return Ok();
        }

        [HttpGet("my-applications")]
        public async Task<IActionResult> GetMyApplications()
        {
            var requesterId = GetUserId();

            var result = await _jobApplicationService.GetMyApplicationsAsync(requesterId);

            return Ok(result);
        }

        [HttpGet("{applicationId:guid}")]
        public async Task<IActionResult> GetDetails(Guid applicationId)
        {
            var requesterId = GetUserId();

            var result = await _jobApplicationService.GetDetailsAsync(requesterId, applicationId);

            return Ok(result);
        }

        [HttpDelete("{applicationId:guid}")]
        public async Task<IActionResult> Cancel(Guid applicationId)
        {
            var requesterId = GetUserId();

            await _jobApplicationService.CancelAsync(requesterId, applicationId);

            return NoContent();
        }

        private Guid GetUserId()
        {
            return Guid.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
    }
