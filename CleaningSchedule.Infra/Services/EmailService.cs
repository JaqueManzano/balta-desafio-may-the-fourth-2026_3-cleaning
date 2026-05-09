using CleaningSchedule.Core.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace CleaningSchedule.Infra.Services
{
    public class EmailService(ILogger<EmailService> logger) : IEmailService
    {
        public async Task SendAsync(string toName, string toEmail, string subject, string body, CancellationToken cancellationToken)
        {
            await Task.Delay(150, cancellationToken);
            logger.LogInformation($"• Enviando lembrete de tarefas para {toName} - ({toEmail}).", cancellationToken);
        }
    }
}
