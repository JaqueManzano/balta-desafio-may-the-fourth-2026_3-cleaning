using CleaningSchedule.Core.Models;
using CleaningSchedule.Core.Repositories.Abstractions;
using System.Collections.Concurrent;

namespace CleaningSchedule.Infra.Repositories
{
    public class MaintenanceTaskRepository : IMaintenanceTaskRepository
    {
        private static readonly ConcurrentDictionary<Guid, MaintenanceTask> MaintenanceTasks = new();

        public Task<IReadOnlyCollection<MaintenanceTask>> GetAllAsync(CancellationToken cancellationToken)
        {
            var tasks = MaintenanceTasks.Values
                .OrderBy(task => task.Title)
                .ToArray();

            return Task.FromResult<IReadOnlyCollection<MaintenanceTask>>(tasks);
        }

        public Task<IEnumerable<MaintenanceTask>> GetPendingTasksAsync(CancellationToken cancellationToken)
        {
            var pendingTasks = MaintenanceTasks.Values
                .Where(task =>
                    task.Active &&
                    task.LastPerformedAt.HasValue &&
                    task.LastPerformedAt.Value.AddDays(task.FrequencyDays) <= DateTime.UtcNow)
                .OrderBy(task => task.LastPerformedAt)
                .ToArray();

            return Task.FromResult<IEnumerable<MaintenanceTask>>(pendingTasks);
        }

        public Task<MaintenanceTask> CreateAsync(MaintenanceTask maintenanceTask, CancellationToken cancellationToken)
        {
            var taskToSave = new MaintenanceTask
            {
                Id = maintenanceTask.Id == Guid.Empty
                    ? Guid.NewGuid()
                    : maintenanceTask.Id,

                Title = maintenanceTask.Title,
                Description = maintenanceTask.Description,
                FrequencyDays = maintenanceTask.FrequencyDays,
                LastPerformedAt = maintenanceTask.LastPerformedAt,
                Active = maintenanceTask.Active
            };

            MaintenanceTasks[taskToSave.Id] = taskToSave;

            return Task.FromResult(taskToSave);
        }

        public Task<MaintenanceTask?> UpdateAsync(MaintenanceTask maintenanceTask, CancellationToken cancellationToken)
        {
            if (!MaintenanceTasks.TryGetValue(maintenanceTask.Id, out var currentTask))
                return Task.FromResult<MaintenanceTask?>(null);

            currentTask.Title = maintenanceTask.Title;
            currentTask.Description = maintenanceTask.Description;
            currentTask.FrequencyDays = maintenanceTask.FrequencyDays;
            currentTask.LastPerformedAt = maintenanceTask.LastPerformedAt;
            currentTask.Active = maintenanceTask.Active;

            return Task.FromResult<MaintenanceTask?>(currentTask);
        }
    }
}