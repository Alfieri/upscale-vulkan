namespace UpscaleVulkan.Exceptions
{
    using System;

    public class ScalingFailedException : Exception
    {
        public ScalingFailedException(string message)
            : base(message)
        {
        }
    }
}