using System;
using System.Linq;
using Spritastic.Generator;
using Spritastic.Utilities;

namespace Spritastic
{
    public class SpriteGenerator
    {
        private readonly ICssImageTransformer cssImageTransformer;
        private readonly Func<ISpriteManager> createSpriteManager;

        public SpriteGenerator(ICssImageTransformer cssImageTransformer, Func<ISpriteManager> createSpriteManager)
        {
            this.cssImageTransformer = cssImageTransformer;
            this.createSpriteManager = createSpriteManager;
        }

        public SpritePackage GenerateFromCss(string cssContent)
        {
            var newImages = cssImageTransformer.ExtractImageUrls(cssContent);
            using (var spriteManager = createSpriteManager())
            {
                foreach (var imageUrl in newImages)
                {
                    Tracer.Trace("Adding {0}", imageUrl.ImageUrl);
                    spriteManager.Add(imageUrl);
                    Tracer.Trace("Finished adding {0}", imageUrl.ImageUrl);
                }
                var sprites = spriteManager.Flush();
                var newCss = spriteManager.Aggregate(cssContent,
                                                     (current, spritedImage) =>
                                                     cssImageTransformer.InjectSprite(current,
                                                                                      spritedImage));
                return new SpritePackage(newCss, sprites, spriteManager.Errors);
            }
        }
    }
}
