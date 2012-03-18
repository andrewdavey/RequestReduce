using System.Collections.Generic;
using Spritastic.ImageLoad;
using Spritastic.Parser;

namespace Spritastic.Generator
{
    public interface ISpriteContainer : IEnumerable<SpritedImage>
    {
        SpritedImage AddImage (BackgroundImageClass image);
        void AddImage(SpritedImage image);
        int Size { get; }
        int Colors { get; }
        int Width { get; }
        int Height { get; }
        IImageLoader ImageLoader { get; set; }
    }
}