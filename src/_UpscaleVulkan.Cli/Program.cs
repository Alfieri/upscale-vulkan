namespace UpscaleVulkan.Cli;

public static class Program
{
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

    private static Task MainAsync(string[] args)
    {
        return Task.CompletedTask;
    }
}