namespace UpscaleVulkan.Test
{
    using System.Threading.Tasks;
    using Dtos;
    using FluentAssertions;
    using Moq;
    using UpscaleVulkan.Core;
    using Xunit;

    public class UpscalerTest
    {
        [Fact]
        public void ShouldUpscale()
        {
            var waifu2X = new Mock<IWaifu2x>();
            var video = new InternalVideo();
            var upscaler = new Upscaler(new UpscaleSettings());

            upscaler.Upscale(video, waifu2X.Object);

            video.UpscaleCalled.Should().BeTrue();
        }

        private class InternalVideo : Video
        {
            public bool UpscaleCalled { get; set; } = false; 
            
            public override Task Upscale(IWaifu2x waifu2X)
            {
                this.UpscaleCalled = true;
                return Task.CompletedTask;
            }
        }
    }
}