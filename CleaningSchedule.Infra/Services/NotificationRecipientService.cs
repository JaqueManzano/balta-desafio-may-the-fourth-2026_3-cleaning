using CleaningSchedule.Core.Models;
using CleaningSchedule.Core.Repositories.Abstractions;
using CleaningSchedule.Core.Services.Abstractions;

namespace CleaningSchedule.Core.Services
{
    public class NotificationRecipientService : INotificationRecipientService
    {
        private readonly INotificationRecipientRepository _notificationRecipientRepository;

        public NotificationRecipientService(
            INotificationRecipientRepository notificationRecipientRepository)
        {
            _notificationRecipientRepository = notificationRecipientRepository;
        }

        public Task<IReadOnlyCollection<NotificationRecipient>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return _notificationRecipientRepository.GetAllAsync(cancellationToken);
        }

        public Task<NotificationRecipient?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return _notificationRecipientRepository.GetByIdAsync(id, cancellationToken);
        }

        public Task<NotificationRecipient> CreateAsync(
            NotificationRecipient notificationRecipient,
            CancellationToken cancellationToken)
        {
            return _notificationRecipientRepository.CreateAsync(
                notificationRecipient,
                cancellationToken);
        }

        public Task<NotificationRecipient?> UpdateAsync(
            NotificationRecipient notificationRecipient,
            CancellationToken cancellationToken)
        {
            return _notificationRecipientRepository.UpdateAsync(
                notificationRecipient,
                cancellationToken);
        }
    }
}