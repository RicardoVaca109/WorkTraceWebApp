using Microsoft.AspNetCore.Mvc;
using WorkTrace.WebApp.Filters;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Controllers
{
    [AuthorizeSession]
    public class AssignmentController : Controller
    {
        private readonly IAssignmentApiService _assignmentApiService;

        public AssignmentController(IAssignmentApiService assignmentApiService)
        {
            _assignmentApiService = assignmentApiService;
        }

        [HttpGet("Assignment/GetAssignmentsList/by-user/{userId}")]
        public async Task<IActionResult> GetAssignmentsList(string userId)
        {
            try
            {
                var assignments = await _assignmentApiService.GetAssignmentsListAsync(userId);
                if (assignments == null) return NotFound();
                return Ok(assignments);
            }
            catch
            {
                return StatusCode(500, "Error retrieving assignments.");
            }
        }

        [HttpGet("Assignment/GetAssignmentTracking/tracking/{assignmentId}")]
        public async Task<IActionResult> GetAssignmentTracking(string assignmentId)
        {
            try
            {
                var tracking = await _assignmentApiService.GetAssignmentTrackingAsync(assignmentId);
                if (tracking == null) return NotFound();
                return Ok(tracking);
            }
            catch
            {
                return StatusCode(500, "Error retrieving tracking info.");
            }
        }
    }
}
