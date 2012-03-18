using System;
using System.Collections.Generic;
using Spritastic.ImageLoad;
using Spritastic.Parser;
using Spritastic.SpriteStore;

namespace Spritastic.Generator
{
    public interface ISpriteManager : IEnumerable<SpritedImage>, IDisposable
    {
        void Add(BackgroundImageClass imageUrl);
        void Flush();
        ISpriteStore SpriteStore { get;  set; }
        IImageLoader ImageLoader { get; set; }
        Predicate<BackgroundImageClass> ImageExclusionFilter { get; set; }
    }
}