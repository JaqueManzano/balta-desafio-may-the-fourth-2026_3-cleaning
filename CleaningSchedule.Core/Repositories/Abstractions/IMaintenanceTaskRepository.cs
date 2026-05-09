using CleaningSchedule.Core.Models;

namespace CleaningSchedule.Core.Repositories.Abstractions
{
    public interface IMaintenanceTaskRepository
    {
        Task<IReadOnlyCollection<MaintenanceTask>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<MaintenanceTask>> GetPendingTasksAsync(CancellationToken cancellationToken);
        Task<MaintenanceTask> CreateAsync(MaintenanceTask maintenanceTask, CancellationToken cancellationToken);
        Task<MaintenanceTask?> UpdateAsync(MaintenanceTask maintenanceTask, CancellationToken cancellationToken);
    }
}
