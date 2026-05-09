using CleaningSchedule.Core.Agents.Abstractions;
using CleaningSchedule.Core.Enums;
using CleaningSchedule.Core.Models;
using CleaningSchedule.Core.Repositories.Abstractions;
using CleaningSchedule.Core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CleaningSchedule.Core.Services
{
    public class MaintenanceTaskService : IMaintenanceTaskService
    {
        private readonly ILogger<MaintenanceTask> _logger;
        private readonly IMaintenanceTaskRepository _maintenanceTaskRepository;
        private readonly IAgent<IEnumerable<MaintenanceTask>, string> _titleGeneratorAgent;
        private readonly IAgent<IEnumerable<MaintenanceTask>, string> _maintenanceTaskAgent;
        private readonly IEmailService _emailService;
        private readonly INotificationRecipientService _notificationRecipientService;

        public MaintenanceTaskService(ILogger<MaintenanceTask> logger, 
            IMaintenanceTaskRepository maintenanceTaskRepository,
            [FromKeyedServices(AgentType.TitleGenerator)] 
            IAgent<IEnumerable<MaintenanceTask>, string> titleGeneratorAgent,
            [FromKeyedServices(AgentType.MaintenanceTaskAgent)] 
            IAgent<IEnumerable<MaintenanceTask>, string> maintenanceTaskAgent,
            IEmailService emailService,
            INotificationRecipientService notificationRecipientService) 
        {
            _logger = logger;
            _maintenanceTaskRepository = maintenanceTaskRepository;
            _titleGeneratorAgent = titleGeneratorAgent;
            _maintenanceTaskAgent = maintenanceTaskAgent;
            _emailService = emailService;
            _notificationRecipientService = notificationRecipientService;
        }

        public async Task SendAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"• Recuperando as tarefas...");
            var tasks = await _maintenanceTaskRepository.GetPendingTasksAsync(cancellationToken);

            if(!tasks.Any())
                return;

            _logger.LogInformation($"• Gerando o título da notificação...");
            var subject = await _titleGeneratorAgent.RunAsync(tasks, cancellationToken);

            _logger.LogInformation($"• Gerando o conteúdo do e-mail das notificações...");
            var body = await _maintenanceTaskAgent.RunAsync(tasks, cancellationToken);

            _logger.LogInformation($"• Recuperando os e-mails...");
            var emails = await _notificationRecipientService.GetAllAsync(cancellationToken);

            _logger.LogInformation($"• Enviando as notificações de tarefas de manutenção da casa....");
            foreach (var email in emails)
                await _emailService.SendAsync(email.Name, email.Email, subject, body, cancellationToken); 
        }

        public Task<IReadOnlyCollection<MaintenanceTask>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            return _maintenanceTaskRepository.GetAllAsync(cancellationToken);
        }

        public Task<IEnumerable<MaintenanceTask>> GetPendingTasksAsync(
            CancellationToken cancellationToken)
        {
            return _maintenanceTaskRepository.GetPendingTasksAsync(cancellationToken);
        }

        public Task<MaintenanceTask> CreateAsync(
            MaintenanceTask maintenanceTask,
            CancellationToken cancellationToken)
        {
            return _maintenanceTaskRepository.CreateAsync(
                maintenanceTask,
                cancellationToken);
        }

        public Task<MaintenanceTask?> UpdateAsync(
            MaintenanceTask maintenanceTask,
            CancellationToken cancellationToken)
        {
            return _maintenanceTaskRepository.UpdateAsync(
                maintenanceTask,
                cancellationToken);
        }
    }
}