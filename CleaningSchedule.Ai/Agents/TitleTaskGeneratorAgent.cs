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
    public class TitleTaskGeneratorAgent : IAgent<IEnumerable<MaintenanceTask>, string>
    {
        private const string AgentName = "TitleTaskGeneratorAgent";

        private readonly IPromptProvider _promptProvider;
        private readonly OllamaApiClient _client;
        private const float Temperature = 0.1f;
        private ILogger<MaintenanceTaskGeneratorAgent> _logger;

        public TitleTaskGeneratorAgent(ILogger<MaintenanceTaskGeneratorAgent> logger, [FromKeyedServices(PromptProvider.File)] IPromptProvider promptProvider)
        {
            _logger = logger;
            _promptProvider = promptProvider;

            _client = OllamaClientFactory.Create();
        }

        public async Task<string> RunAsync(IEnumerable<MaintenanceTask> data, CancellationToken cancellationToken)
        {
            _logger.LogInformation("• Gerando título das notificações das tarefas...");

            var instructions = await _promptProvider.GetPromptAsync(AgentName, cancellationToken);
            var prompt = $"{instructions} - {data}";

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

            _logger.LogInformation("• Títulos de notificações geradas...");
            _logger.LogInformation("---");
            _logger.LogInformation(finalResponse);
            _logger.LogInformation("---");

            return finalResponse;
        }
    }
}
