using System.Collections.Generic;

namespace Spriting
{
    public interface ISpriteContainer : IEnumerable<SpritedImage>
    {
        SpritedImage AddImage (BackgroundImageClass image);
        void AddImage(SpritedImage image);
        int Size { get; }
        int Colors { get; }
        int Width { get; }
        int Height { get; }
    }
}