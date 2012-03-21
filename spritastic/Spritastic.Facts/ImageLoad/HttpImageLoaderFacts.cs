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
            [InlineDataAttribute("image.png", "http://host.com", "http://host.com/image.png")]
            [InlineDataAttribute("../images/image.png", "http://host.com/styles/style.css", "http://host.com/images/image.png")]
            [InlineDataAttribute("http://external.com/image.png", "http://host.com", "http://external.com/image.png")]
            [InlineDataAttribute("/images/image.png", "http://host.com/styles/style.css", "http://host.com/images/image.png")]
            public void WillCreateAbsoluteUrlFromGivenUrl(string url, string baseUrl, string expectedUrl)
            {
                var testable = new TestableHttpImageLoader();
                var urlCalled = string.Empty;
                testable.Mock<IWebClientWrapper>().Setup(x => x.DownloadBytes(It.IsAny<string>())).Callback<string>(
                    urlParam => urlCalled = urlParam);
                testable.ClassUnderTest.BasePath = baseUrl;

                testable.ClassUnderTest.GetImageBytes(url);

                Assert.Equal(expectedUrl, urlCalled);
            }
        }
    }
}
