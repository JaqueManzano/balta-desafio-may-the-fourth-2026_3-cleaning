using CleaningSchedule.Core.Models;

namespace CleaningSchedule.Core.Repositories.Abstractions
{
    public interface INotificationRecipientRepository
    {
        Task<IReadOnlyCollection<NotificationRecipient>> GetAllAsync(
            CancellationToken cancellationToken);

        Task<NotificationRecipient?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<NotificationRecipient> CreateAsync(
            NotificationRecipient notificationRecipient,
            CancellationToken cancellationToken);

        Task<NotificationRecipient?> UpdateAsync(
            NotificationRecipient notificationRecipient,
            CancellationToken cancellationToken);
    }
}
