using CleaningSchedule.Core.Repositories.Abstractions;
using CleaningSchedule.Core.Services;
using CleaningSchedule.Core.Services.Abstractions;
using CleaningSchedule.Infra.Repositories;
using CleaningSchedule.Infra.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleaningSchedule.Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<INotificationRecipientService, NotificationRecipientService>();
        services.AddScoped<IMaintenanceTaskService, MaintenanceTaskService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IMaintenanceTaskRepository, MaintenanceTaskRepository>();
        services.AddScoped<INotificationRecipientRepository, NotificationRecipientRepository>();

        return services;
    }
}