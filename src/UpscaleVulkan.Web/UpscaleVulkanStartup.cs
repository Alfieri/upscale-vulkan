using UpscaleVulkan.Web.Services;

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
            services.AddViewServices();
            services.AddApplicationServices();

            return services;
        }

        private static IServiceCollection AddViewServices(this IServiceCollection services)
        {
            services.AddSingleton<IVideoInfoService, VideoInfoService>();

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