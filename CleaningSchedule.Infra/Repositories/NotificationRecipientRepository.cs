using CleaningSchedule.Core.Models;
using CleaningSchedule.Core.Repositories.Abstractions;
using System.Collections.Concurrent;

namespace CleaningSchedule.Infra.Repositories
{
    public class NotificationRecipientRepository : INotificationRecipientRepository
    {
        private static readonly ConcurrentDictionary<Guid, NotificationRecipient> NotificationRecipients = new();

        public Task<IReadOnlyCollection<NotificationRecipient>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var recipients = NotificationRecipients.Values
                .OrderBy(recipient => recipient.Name)
                .ToArray();

            return Task.FromResult<IReadOnlyCollection<NotificationRecipient>>(recipients);
        }

        public Task<NotificationRecipient?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            NotificationRecipients.TryGetValue(id, out var recipient);

            return Task.FromResult(recipient);
        }

        public Task<NotificationRecipient> CreateAsync(
            NotificationRecipient notificationRecipient,
            CancellationToken cancellationToken)
        {
            var recipientToSave = new NotificationRecipient
            {
                Id = notificationRecipient.Id == Guid.Empty
                    ? Guid.NewGuid()
                    : notificationRecipient.Id,

                Name = notificationRecipient.Name,
                Email = notificationRecipient.Email,
                Active = notificationRecipient.Active
            };

            NotificationRecipients[recipientToSave.Id] = recipientToSave;

            return Task.FromResult(recipientToSave);
        }

        public Task<NotificationRecipient?> UpdateAsync(
            NotificationRecipient notificationRecipient,
            CancellationToken cancellationToken)
        {
            if (!NotificationRecipients.TryGetValue(notificationRecipient.Id, out var currentRecipient))
                return Task.FromResult<NotificationRecipient?>(null);

            currentRecipient.Name = notificationRecipient.Name;
            currentRecipient.Email = notificationRecipient.Email;
            currentRecipient.Active = notificationRecipient.Active;

            return Task.FromResult<NotificationRecipient?>(currentRecipient);
        }
    }
}