namespace UpscaleVulkan.Reporting
{
    using System;
    using Core;
    using Microsoft.Extensions.Logging;

    public class TimeEstimation
    {
        private readonly ILogger<TimeEstimation> _logger;
        private DateTime _scaleStartTime;

        public TimeEstimation(Video video, ILogger<TimeEstimation> logger)
        {
            this._logger = logger;
            video.ScalingStarted += this.OnScalingStarted;
            video.ScalingFinished += this.OnScalingFinished;
        }

        private void OnScalingFinished(object sender, ScaleReportingEventArgs e)
        {
            var batchTime = DateTime.Now - this._scaleStartTime;
            TimeSpan estimatedTime = ((e.NumberOfFrames - e.CurrentFrame) / e.BatchSize) * batchTime;
            this._logger.LogInformation($"upscaling frames takes {batchTime}");
            this._logger.LogInformation($"Scaling finished in {estimatedTime}");
        }

        private void OnScalingStarted(object sender, EventArgs e)
        {
            this._scaleStartTime = DateTime.Now;
        }
    }
}