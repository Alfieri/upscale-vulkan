namespace UpscaleVulkan.Test.Core
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Moq;
    using UpscaleVulkan.Core;
    using Xunit;

    public class FrameTest
    {
        [Fact]
        public void ShouldPassParameters()
        {
            const string FrameName = "testframe";
            const string FramePath = "just/a/path";
            const bool IsUpscaled = true;

            var frame = new Frame(FramePath, FrameName, IsUpscaled);

            frame.Should().NotBeNull();
        }

        [Fact]
        public async Task ShouldUpscaleFrame()
        {
            var waifu2X = new Mock<IWaifu2x>();
            var result = new ScaledFrame(new Frame(string.Empty, string.Empty, true));
            waifu2X.Setup(w => w.Upscale(It.IsAny<Frame>()))
                .Returns(Task.FromResult(result));
            
            var frame = new Frame(string.Empty, string.Empty, false);
            ScaledFrame scaledFrame = await frame.Upscale(waifu2X.Object);

            waifu2X.Verify(w => w.Upscale(frame), Times.Once);
            scaledFrame.Should().NotBeNull();
            scaledFrame.Should().Be(result);
        }

        [Fact]
        public async Task ShouldNotUpscaleAlreadyScaledFrame()
        {
            var waifu2X = new Mock<IWaifu2x>();
            
            var frame = new Frame(string.Empty, string.Empty, true);
            ScaledFrame scaledFrame = await frame.Upscale(waifu2X.Object);
            
            waifu2X.Verify(w => w.Upscale(frame), Times.Never);
            scaledFrame.Should().NotBeNull();
        }
    }
}