using CleaningSchedule.Core.Models;

namespace CleaningSchedule.Core.Services.Abstractions
{
    public interface INotificationRecipientService
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