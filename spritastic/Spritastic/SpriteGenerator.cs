using System;
using System.Linq;
using Spritastic.Generator;
using Spritastic.Utilities;

namespace Spritastic
{
    public class SpriteGenerator
    {
        private readonly ICssImageExtractor cssImageExtractor;
        private readonly Func<ISpriteManager> createSpriteManager;

        public SpriteGenerator(ICssImageExtractor cssImageExtractor, Func<ISpriteManager> createSpriteManager)
        {
            this.cssImageExtractor = cssImageExtractor;
            this.createSpriteManager = createSpriteManager;
        }

        public SpritePackage GenerateFromCss(string cssContent)
        {
            var newImages = cssImageExtractor.ExtractImageUrls(cssContent);
            using (var spriteManager = createSpriteManager())
            {
                foreach (var imageUrl in newImages)
                {
                    Tracer.Trace("Adding {0}", imageUrl.ImageUrl);
                    spriteManager.Add(imageUrl);
                    Tracer.Trace("Finished adding {0}", imageUrl.ImageUrl);
                }
                var sprites = spriteManager.Flush();
                var newCss = spriteManager.Aggregate(
                    cssContent,
                    (current, spritedImage) => spritedImage.InjectIntoCss(current)
                );
                return new SpritePackage(newCss, sprites, spriteManager.Errors);
            }
        }
    }
}
