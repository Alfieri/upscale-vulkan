namespace UpscaleVulkan.Cli
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;


    public class Program
    {
        private static IConfigurationRoot configuration;
        
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

        public static Task MainAsync(string[] args)
        {
            return Task.CompletedTask;
        }
    }
}
