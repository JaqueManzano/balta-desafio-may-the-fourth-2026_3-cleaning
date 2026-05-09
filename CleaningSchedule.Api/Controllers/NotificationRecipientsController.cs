using CleaningSchedule.Core.Models;
using CleaningSchedule.Core.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CleaningSchedule.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationRecipientsController : ControllerBase
    {
        private readonly INotificationRecipientService _notificationRecipientService;

        public NotificationRecipientsController(
            INotificationRecipientService notificationRecipientService)
        {
            _notificationRecipientService = notificationRecipientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var recipients = await _notificationRecipientService
                .GetAllAsync(cancellationToken);

            return Ok(recipients);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var recipient = await _notificationRecipientService
                .GetByIdAsync(id, cancellationToken);

            if (recipient is null)
                return NotFound();

            return Ok(recipient);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] NotificationRecipient notificationRecipient,
            CancellationToken cancellationToken)
        {
            var createdRecipient = await _notificationRecipientService
                .CreateAsync(notificationRecipient, cancellationToken);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdRecipient.Id },
                createdRecipient);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] NotificationRecipient notificationRecipient,
            CancellationToken cancellationToken)
        {
            if (id != notificationRecipient.Id)
                return BadRequest();

            var updatedRecipient = await _notificationRecipientService
                .UpdateAsync(notificationRecipient, cancellationToken);

            if (updatedRecipient is null)
                return NotFound();

            return Ok(updatedRecipient);
        }
    }
}