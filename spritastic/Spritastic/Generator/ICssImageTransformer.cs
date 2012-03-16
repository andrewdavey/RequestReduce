using System.Collections.Generic;
using Spritastic.Parser;

namespace Spritastic.Generator
{
    public interface ICssImageTransformer
    {
        IEnumerable<BackgroundImageClass> ExtractImageUrls(string cssContent);
        string InjectSprite(string originalCss, SpritedImage image);
    }
}