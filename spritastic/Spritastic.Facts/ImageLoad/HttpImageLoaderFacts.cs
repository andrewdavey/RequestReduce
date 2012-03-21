using System;
using Moq;
using Spritastic.Facts.Utilities;
using Spritastic.ImageLoad;
using Spritastic.Utilities;
using Xunit;
using Xunit.Extensions;

namespace Spritastic.Facts.ImageLoad
{
    public class HttpImageLoaderFacts
    {
        class TestableHttpImageLoader : Testable<HttpImageLoader>
        {
            public TestableHttpImageLoader()
            {
                
            }
        }

        class GetImageBytes
        {
            [Theory]
            [InlineDataAttribute("image.png", "http://host.com/image.png")]
            public void WillCreateAbsoluteUrlFromGivenUrl(string url, string expectedUrl)
            {
                var testable = new TestableHttpImageLoader();
                var urlCalled = string.Empty;
                testable.Mock<IWebClientWrapper>().Setup(x => x.DownloadBytes(It.IsAny<string>())).Callback<string>(
                    urlParam => urlCalled = urlParam);
                testable.ClassUnderTest.BasePath = new Uri(expectedUrl).Host;

                testable.ClassUnderTest.GetImageBytes(url);

                Assert.Equal(expectedUrl, urlCalled);
            }
        }
    }
}
