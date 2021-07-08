namespace UpscaleVulkan.Web
{
    using Application.Persistence;
    using Application.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class UpscaleVulkanStartup
    {
        public static IServiceCollection AddUpscaleVulkanServices(this IServiceCollection services)
        {
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
            
            return services;
        }
    }
}