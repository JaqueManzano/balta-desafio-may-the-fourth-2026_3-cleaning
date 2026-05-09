namespace CleaningSchedule.Core.Models
{
    public class MaintenanceTask
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Task Name
        /// Ex: Change the water filter
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Optional description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Ideal frequency per day
        /// </summary>
        public int FrequencyDays { get; set; }

        /// <summary>
        /// Last time maintenance was performed
        /// </summary>
        public DateTime? LastPerformedAt { get; set; }

        /// <summary>
        /// Indicates whether the task is active
        /// </summary>
        public bool Active { get; set; } = true;
    }
}
