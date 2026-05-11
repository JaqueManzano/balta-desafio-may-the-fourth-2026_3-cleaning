using CleaningSchedule.Ai.Agents;
using CleaningSchedule.Ai.Providers;
using CleaningSchedule.Ai.Providers.Abstractions;
using CleaningSchedule.Core.Agents.Abstractions;
using CleaningSchedule.Core.Enums;
using CleaningSchedule.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CleaningSchedule.Ai;

public static class DependencyInjection
{
    public static IServiceCollection AddAgents(this IServiceCollection services)
    {
        services.AddKeyedTransient<IAgent<IEnumerable<MaintenanceTask>, string>, TitleTaskGeneratorAgent>(AgentType.TitleGenerator);
        services.AddKeyedTransient<IAgent<IEnumerable<MaintenanceTask>, string>, MaintenanceTaskGeneratorAgent>(AgentType.MaintenanceTaskAgent);
        
        services.AddKeyedTransient<IPromptProvider, FilePromptProvider>(PromptProvider.File);

        return services;
    }
}