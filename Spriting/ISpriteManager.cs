using System;
using System.Collections.Generic;

namespace Spriting
{
    public interface ISpriteManager : IEnumerable<SpritedImage>, IDisposable
    {
        void Add(BackgroundImageClass imageUrl);
        void Flush();
    }
}