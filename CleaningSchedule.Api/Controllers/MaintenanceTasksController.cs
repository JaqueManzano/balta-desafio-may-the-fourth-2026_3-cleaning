using CleaningSchedule.Core.Models;
using CleaningSchedule.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CleaningSchedule.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceTasksController : ControllerBase
    {
        private readonly IMaintenanceTaskService _maintenanceTaskService;

        public MaintenanceTasksController(
            IMaintenanceTaskService maintenanceTaskService)
        {
            _maintenanceTaskService = maintenanceTaskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var tasks = await _maintenanceTaskService
                .GetAllAsync(cancellationToken);

            return Ok(tasks);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending(
            CancellationToken cancellationToken)
        {
            var tasks = await _maintenanceTaskService
                .GetPendingTasksAsync(cancellationToken);

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] MaintenanceTask maintenanceTask,
            CancellationToken cancellationToken)
        {
            var createdTask = await _maintenanceTaskService
                .CreateAsync(maintenanceTask, cancellationToken);

            return Ok(createdTask);
        }

        [HttpPut]
        public async Task<IActionResult> Update(
            [FromBody] MaintenanceTask maintenanceTask,
            CancellationToken cancellationToken)
        {
            var updatedTask = await _maintenanceTaskService
                .UpdateAsync(maintenanceTask, cancellationToken);

            if (updatedTask is null)
                return NotFound();

            return Ok(updatedTask);
        }
    }
}