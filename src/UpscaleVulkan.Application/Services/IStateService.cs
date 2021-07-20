namespace UpscaleVulkan.Application.Services
{
    using System.Threading.Tasks;
    using States;

    public interface IStateService
    {
        Task<State> GetCurrentState();
    }
}