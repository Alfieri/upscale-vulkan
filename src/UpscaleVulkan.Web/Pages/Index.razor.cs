namespace UpscaleVulkan.Web.Pages;

using Microsoft.AspNetCore.Components;
    
public partial class Index
{
    [Inject] private NavigationManager navigationManager { get; set; }

    protected override void OnInitialized()
    {
        this.navigationManager.NavigateTo("upscale");
    }
}