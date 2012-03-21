using Spritastic.Utilities;

namespace Spritastic.ImageLoad
{
    public class HttpImageLoader : IImageLoader
    {
        protected readonly IWebClientWrapper WebClientWrapper;

        public HttpImageLoader(IWebClientWrapper webClientWrapper)
        {
            WebClientWrapper = webClientWrapper;
        }

        public string BasePath { get; set; }

        public byte[] GetImageBytes(string url)
        {
            return new byte[0];
        }
    }
}