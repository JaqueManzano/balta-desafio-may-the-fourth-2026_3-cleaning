namespace CleaningSchedule.Core.Models
{
    public class NotificationRecipient
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Person's name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Email address for receiving notifications
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Recebe notificações ativas
        /// </summary>
        public bool Active { get; set; } = true;
    }
}