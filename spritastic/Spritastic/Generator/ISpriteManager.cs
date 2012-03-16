using System;
using System.Collections.Generic;
using Spritastic.Parser;

namespace Spritastic.Generator
{
    public interface ISpriteManager : IEnumerable<SpritedImage>, IDisposable
    {
        void Add(BackgroundImageClass imageUrl);
        void Flush();
    }
}