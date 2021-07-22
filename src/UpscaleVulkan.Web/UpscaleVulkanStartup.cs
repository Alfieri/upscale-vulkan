namespace UpscaleVulkan.Web
{
    using Microsoft.Extensions.DependencyInjection;
    
    using Application.Infrastructure;
    using Application.Persistence;
    using Application.Services;
    using Services;

    public static class UpscaleVulkanStartup
    {
        public static IServiceCollection AddUpscaleVulkanServices(this IServiceCollection services)
        {
            services.AddViewServices();
            services.AddApplicationServices();

            return services;
        }

        private static IServiceCollection AddViewServices(this IServiceCollection services)
        {
            services.AddSingleton<IUpscaleComponentViewService, UpscaleComponentViewService>();

            return services;
        }

        private static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IStateService, StateService>();
            services.AddSingleton<ISettingsService, SettingsService>();
            services.AddSingleton<IVideoConverter, FfmpegService>();
            services.AddSingleton<IWaifu2x, Waifu2xVulkan>();
            services.AddSingleton<IFileProxy, File>();
            
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
            
            
            return services;
        }
    }
}