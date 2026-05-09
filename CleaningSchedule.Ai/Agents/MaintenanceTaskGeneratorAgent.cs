using CleaningSchedule.Ai.Models;
using CleaningSchedule.Ai.Providers.Abstractions;
using CleaningSchedule.Core.Agents.Abstractions;
using CleaningSchedule.Core.Enums;
using CleaningSchedule.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using OllamaSharp.Models;

namespace CleaningSchedule.Ai.Agents
{
    public class MaintenanceTaskGeneratorAgent : IAgent<IEnumerable<MaintenanceTask>, string>
    {
        private const string AgentName = "MaintenanceTaskGeneratorAgent";

        private readonly IPromptProvider _promptProvider;
        private readonly OllamaApiClient _client;
        private const float Temperature = 0.1f;
        private ILogger<MaintenanceTaskGeneratorAgent> _logger;

        public MaintenanceTaskGeneratorAgent(ILogger<MaintenanceTaskGeneratorAgent> logger, [FromKeyedServices(PromptProvider.File)] IPromptProvider promptProvider)
        {
            _logger = logger;
            _promptProvider = promptProvider;

            _client = OllamaClientFactory.Create();
        }

        public async Task<string> RunAsync(IEnumerable<MaintenanceTask> data,CancellationToken cancellationToken)
        {
            _logger.LogInformation("• Gerando a notificação das tarefas...");

            var instructions = await _promptProvider
                .GetPromptAsync(AgentName, cancellationToken);

            var tasksText = string.Join(
                Environment.NewLine + Environment.NewLine,
                data.Select((task, index) => $"""
                Nome: {task.Title}
                Descrição: {task.Description}
                Última manutenção: {(task.LastPerformedAt.HasValue
                                ? task.LastPerformedAt.Value.ToString("dd/MM/yyyy")
                                : "Não informada")}
                Frequência: a cada {task.FrequencyDays} dias
                """));
                
                var prompt = $"""
                {instructions}
                
                Lista de tarefas:
                
                {tasksText}
                """;

            var finalResponse = string.Empty;

            await foreach (var chunk in _client.GenerateAsync(
                               new GenerateRequest()
                               {
                                   Prompt = prompt,
                                   Options = new RequestOptions
                                   {
                                       Temperature = Temperature
                                   }
                               },
                               cancellationToken))
            {
                if (!string.IsNullOrWhiteSpace(chunk?.Response))
                    finalResponse += chunk.Response;
            }

            _logger.LogInformation("• Notificações geradas...");
            _logger.LogInformation("---");
            _logger.LogInformation(finalResponse);
            _logger.LogInformation("---");

            return finalResponse;
        }
    }
}
