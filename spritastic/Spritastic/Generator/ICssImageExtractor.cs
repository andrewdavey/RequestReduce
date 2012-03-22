using System.Collections.Generic;
using Spritastic.Parser;

namespace Spritastic.Generator
{
    public interface ICssImageExtractor
    {
        IEnumerable<BackgroundImageClass> ExtractImageUrls(string cssContent);
    }
}