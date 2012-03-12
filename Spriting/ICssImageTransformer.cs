using System.Collections.Generic;

namespace Spriting
{
    public interface ICssImageTransformer
    {
        IEnumerable<BackgroundImageClass> ExtractImageUrls(string cssContent);
        string InjectSprite(string originalCss, SpritedImage image);
    }
}