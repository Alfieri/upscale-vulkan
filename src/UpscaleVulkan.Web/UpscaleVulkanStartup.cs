namespace UpscaleVulkan.Web
{
    using Application.Infrastructure;
    using Application.Persistence;
    using Application.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class UpscaleVulkanStartup
    {
        public static IServiceCollection AddUpscaleVulkanServices(this IServiceCollection services)
        {
            services.AddSingleton<IUpscaleService, UpscaleService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IVideoConverter, Ffmpeg>();
            services.AddSingleton<IWaifu2x, Waifu2xVulkan>();
            services.AddSingleton<IFileProxy, File>();
            
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
            
            
            return services;
        }
    }
}