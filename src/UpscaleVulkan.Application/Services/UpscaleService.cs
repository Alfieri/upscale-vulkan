namespace UpscaleVulkan.Application.Services
{
    using System.Threading.Tasks;
    using Core;

    public class UpscaleService : IUpscaleService
    {
        private readonly IVideoConverter videoConverter;
        private readonly IWaifu2x waifu2X;

        public UpscaleService(IVideoConverter videoConverter, IWaifu2x waifu2X)
        {
            this.videoConverter = videoConverter;
            this.waifu2X = waifu2X;
        }

        public async Task Upscale(Video video)
        {
            await this.ExtractFrames(video);
            await this.UpscaleFrames(video);
            await this.CombineScaledFrames(video);
        }

        private async Task CombineScaledFrames(Video video)
        {
            var upscaledVideo = await this.videoConverter.CreateVideoFromFrames();
            var intermediateVideo = new IntermediateVideo(video, upscaledVideo);
            await this.videoConverter.CreateFinaleVideo(intermediateVideo);
        }

        private async Task ExtractFrames(Video video)
        {
            var extractFrames = await this.videoConverter.ExtractFrames(video);
            video.AddFrames(extractFrames);
        }

        private async Task UpscaleFrames(Video video)
        {
            foreach (Frame frame in video.GetFrames())
            {
                if (frame.IsAlreadyScaled)
                {
                    continue;
                }

                await this.waifu2X.Upscale(frame);
                frame.SetToUpscaled();
            }
        }
    }
}