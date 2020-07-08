namespace UpscaleVulkan.Console
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Dtos;
    using Externals;
    using Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Configuration;
    using File = Infrastructure.File;

    public class Program
    {
        public static IConfigurationRoot configuration;
        
        public static int Main(string[] args)
        {
            try
            {
                MainAsync(args).Wait();
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public static async Task MainAsync(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            
            ILogger<Program> logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Starting...");

            var upscaleApp = serviceProvider.GetService<UpscaleApp>();
            try
            {
                await upscaleApp.Run(args);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Catch unhandled Exception");
                throw;
            }
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConfiguration();
                builder.AddConsole();
            });
            
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            serviceCollection.AddSingleton<UpscaleSettings>(configuration.GetSection("upscale").Get<UpscaleSettings>());
            serviceCollection.AddSingleton<FfmpegParameter>(configuration.GetSection("ffmpeg").Get<FfmpegParameter>());
            serviceCollection.AddSingleton<Waifu2xSettings>(configuration.GetSection("waifu2xvulkan").Get<Waifu2xSettings>());
            serviceCollection.AddSingleton<IUpscaler, Upscaler>();
            serviceCollection.AddSingleton<IWaifu2x, Waifu2xVulkan>();
            serviceCollection.AddSingleton<IVideoConverter, Ffmpeg>();
            serviceCollection.AddSingleton<IFileProxy, File>();
            serviceCollection.AddSingleton<UpscaleApp>();
        }
    }
}
