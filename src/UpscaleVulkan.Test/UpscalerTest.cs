namespace UpscaleVulkan.Test
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Dtos;
    using FluentAssertions;
    using Moq;
    using UpscaleVulkan.Core;
    using Xunit;

    public class UpscalerTest
    {
        private Mock<IWaifu2x> _waifu2X;
        private InternalVideo _video;
        private Upscaler _upscaler;
        private Mock<IVideoConverter> _videoConverter;

        public UpscalerTest()
        {
            this._videoConverter = new Mock<IVideoConverter>();
            this._waifu2X = new Mock<IWaifu2x>();
            this._video = new InternalVideo();
            this._upscaler = new Upscaler(new UpscaleSettings());
            this._upscaler.SetVideo(this._video);
        }
        
        [Fact]
        public void ShouldUpscale()
        {
            this._upscaler.Upscale(this._waifu2X.Object);

            this._video.UpscaleCalled.Should().BeTrue();
        }

        [Fact]
        public void ShouldExtractFrames()
        {
            this._upscaler.ExtractFrames(this._videoConverter.Object);

            this._video.ExtractCalled.Should().BeTrue();
        }

        [Fact]
        public void ShouldCreateFinaleVideo()
        {
            this._upscaler.CreateFinaleVideo(this._videoConverter.Object);

            this._video.CreateFinaleVideoCalled.Should().BeTrue();
        }

        [Fact]
        public void ShouldCreateVideoFromScaledFrames()
        {
            this._upscaler.CreateVideoFromScaledFrames(this._videoConverter.Object);

            this._video.CreateVideoFromScaledFramesCalled.Should().BeTrue();
        }

        private class InternalVideo : Video
        {
            public bool UpscaleCalled { get; set; } = false;

            public bool ExtractCalled { get; set; } = false;

            public bool CreateFinaleVideoCalled { get; set; } = false;
            
            public bool CreateVideoFromScaledFramesCalled { get; set; } = false;

            public override Task Upscale(IWaifu2x waifu2X)
            {
                this.UpscaleCalled = true;
                return Task.CompletedTask;
            }

            public override Task ExtractFramesFromVideo(IVideoConverter videoConverter)
            {
                this.ExtractCalled = true;
                return Task.CompletedTask;
            }

            public override Task CreateFinaleVideo(IVideoConverter videoConverter)
            {
                this.CreateFinaleVideoCalled = true;
                return Task.CompletedTask;
            }

            public override Task<IntermediateVideo> CreateVideoFromUpscaledFrames(IVideoConverter videoConverter)
            {
                this.CreateVideoFromScaledFramesCalled = true;
                return Task.FromResult(new IntermediateVideo(null));
            }
        }
    }
}