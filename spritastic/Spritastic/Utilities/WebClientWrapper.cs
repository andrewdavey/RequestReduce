using System;
using System.Net;

namespace Spritastic.Utilities
{
    public interface IWebClientWrapper
    {
        byte[] DownloadBytes(string url);
    }

    public class WebClientWrapper : IWebClientWrapper
    {
        private readonly IWebProxy proxy;

        public WebClientWrapper()
        {
            if (TrustLevelChecker.IsFullTrust() && Environment.Version.Major < 4)
                return;
            proxy = WebRequest.GetSystemWebProxy();
            proxy.Credentials = CredentialCache.DefaultCredentials;
        }

        public byte[] DownloadBytes(string url)
        {
            try
            {
                using (var client = new WebClient())
                {
                    if (TrustLevelChecker.IsFullTrust() || Environment.Version.Major >= 4)
                        client.Proxy = proxy;
                    client.Credentials = CredentialCache.DefaultCredentials;
                    return client.DownloadData(url);
                }
            }
            catch (Exception ex)
            {
                throw new SpriteException(
                    string.Format("Spritastic had problems accessing {0}. Error Message from WebClient is: {1}", url,
                                  ex.Message), ex);
            }
        }
    }
}
