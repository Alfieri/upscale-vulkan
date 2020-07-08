namespace UpscaleVulkan.Console
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    public class UpscaleApp
    {
        private readonly IConfigurationRoot _configuration;
        private readonly ILogger<UpscaleApp> _logger;
        private readonly IUpscaler _upscaler;

        public UpscaleApp(IConfigurationRoot configuration, ILogger<UpscaleApp> logger, IUpscaler upscaler)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._upscaler = upscaler;
        }
        
        public Task Run(string[] args)
        {
            return Task.Run(() => this._logger.LogInformation("Hello World"));
        }
    }
}