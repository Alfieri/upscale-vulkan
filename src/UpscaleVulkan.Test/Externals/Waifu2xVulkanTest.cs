namespace UpscaleVulkan.Test.Externals
{
    using System.Threading.Tasks;
    using Dtos;
    using FluentAssertions;
    using UpscaleVulkan.Core;
    using UpscaleVulkan.Externals;
    using Xunit;

    public class Waifu2xVulkanTest
    {
        [Fact]
        public void ShouldCreateWithSettings()
        {
            var waifu2XVulkan = new Waifu2xVulkan(new Waifu2xSettings()
            {
                Executable = "someExe",
                ModelPath = "someModelPath",
                NoiseLevel = 2,
                Scale = 2
            });

            waifu2XVulkan.ProcessStartInfo.Arguments.Should()
                .Contain("-m someModelPath").And
                .Contain("-s 2").And
                .Contain("-n 2");
        }

        [Fact(Skip = "Refactor dependency to System.IO.File")]
        public async Task ShouldUpscaleFrame()
        {
            var frame = new Frame("somePath", "someFile", false);
            
            var waifu2XVulkan = new Waifu2xVulkan(new Waifu2xSettings()
            {
                Executable = "echo",
                OutputPath = "someOutPath"
            });
            waifu2XVulkan.ProcessStartInfo.Arguments = "Hello World";
            ScaledFrame scaledFrame = await waifu2XVulkan.Upscale(frame);
            
            scaledFrame.FrameName.Should().Be(frame.FrameName);
            scaledFrame.FramePath.Should().Be(frame.FramePath);
        }
    }
}