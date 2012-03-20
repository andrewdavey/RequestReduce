using System;
using System.Collections.Generic;
using Spritastic.ImageLoad;
using Spritastic.Parser;

namespace Spritastic.Generator
{
    public interface ISpriteManager : IEnumerable<SpritedImage>, IDisposable
    {
        void Add(BackgroundImageClass imageUrl);
        IList<Sprite> Flush();
        IImageLoader ImageLoader { get; set; }
        Predicate<BackgroundImageClass> ImageExclusionFilter { get; set; }
        IList<SpriteException> Errors { get; }
    }
}