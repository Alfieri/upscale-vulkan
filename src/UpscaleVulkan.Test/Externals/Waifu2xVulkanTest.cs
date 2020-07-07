namespace UpscaleVulkan.Test.Externals
{
    using System;
    using System.Threading.Tasks;
    using Dtos;
    using Exceptions;
    using FluentAssertions;
    using Infrastructure;
    using Moq;
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
            }, new Mock<IFileProxy>().Object);

            waifu2XVulkan.ProcessStartInfo.ArgumentList.Should()
                .Contain("-m someModelPath").And
                .Contain("-s 2").And
                .Contain("-n 2");
        }

        [Fact]
        public async Task ShouldUpscaleFrame()
        {
            var frame = new Frame("somePath", "someFile", false);
            var fileProxy = new Mock<IFileProxy>();
            fileProxy.Setup(f => f.ExistsAsync(It.IsAny<string>())).Returns(Task.FromResult(true));
            var waifu2XVulkan = new Waifu2xVulkan(new Waifu2xSettings()
            {
                Executable = "echo",
                OutputPath = "someOutPath"
            }, fileProxy.Object);
            
            ScaledFrame scaledFrame = await waifu2XVulkan.Upscale(frame);
            
            scaledFrame.FrameName.Should().Be(frame.FrameName);
            scaledFrame.FramePath.Should().Be(frame.FramePath);
        }

        [Fact]
        public void ShouldThrowExceptionIfScalingFailed()
        {
            var frame = new Frame("somePath", "someFile", false);
            var fileProxy = new Mock<IFileProxy>();
            fileProxy.Setup(f => f.ExistsAsync(It.IsAny<string>())).Returns(Task.FromResult(false));
            var waifu2XVulkan = new Waifu2xVulkan(new Waifu2xSettings()
            {
                Executable = "echo",
                OutputPath = "someOutPath"
            }, fileProxy.Object);

            waifu2XVulkan.Invoking(w => w.Upscale(frame)).Should().Throw<ScalingFailedException>();
        }
    }
}