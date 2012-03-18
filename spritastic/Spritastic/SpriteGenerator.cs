using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Spritastic.Generator;
using Spritastic.ImageLoad;
using Spritastic.Parser;
using Spritastic.Selector;
using Spritastic.SpriteStore;
using Spritastic.Utilities;
using nQuant;

namespace Spritastic
{
    public class SpriteGenerator
    {
        private static readonly ICssImageTransformer DefaultCssImageTransformer =
            new CssImageTransformer(new CssSelectorAnalyzer());
        private static readonly IImageLoader DefaultImageLoader;
        private static readonly ISpriteStore DefaultSpriteStore;

        private readonly ISpritingSettings settings;
        private readonly ICssImageTransformer cssImageTransformer;
        private readonly ISpriteManager spriteManager;

        public SpriteGenerator(ISpritingSettings settings)
            : this(settings, DefaultCssImageTransformer, new SpriteManager(settings, DefaultImageLoader, DefaultSpriteStore, new PngOptimizer(new FileWrapper(), new WuQuantizer()) ))
        {
            
        }

        internal SpriteGenerator(ISpritingSettings settings, ICssImageTransformer cssImageTransformer, ISpriteManager spriteManager)
        {
            this.settings = settings;
            this.cssImageTransformer = cssImageTransformer;
            this.spriteManager = spriteManager;
        }

        public SpritePackage GenerateFromCss(string cssContent)
        {
            var newImages = cssImageTransformer.ExtractImageUrls(cssContent);
            foreach (var imageUrl in newImages)
            {
                Tracer.Trace("Adding {0}", imageUrl.ImageUrl);
                spriteManager.Add(imageUrl);
                Tracer.Trace("Finished adding {0}", imageUrl.ImageUrl);
            }
            spriteManager.Dispose();
            var sprites = new List<Sprite>();
            var newCss = spriteManager.Aggregate(cssContent, (current, spritedImage) =>
                                                                 {
                                                                     using (var memStream = new MemoryStream())
                                                                     {
                                                                         spritedImage.Image.Save(memStream, ImageFormat.Png);
                                                                         sprites.Add(new Sprite(spritedImage.Url,
                                                                                                    memStream.GetBuffer()));
                                                                     }
                                                                     return cssImageTransformer.InjectSprite(current,
                                                                                                      spritedImage);
                                                                 });
            return new SpritePackage(newCss, sprites);
        }

        public void RegisterImageLoader(IImageLoader imageLoader)
        {
            spriteManager.ImageLoader = imageLoader;
        }

        public void RegisterSpriteStore(ISpriteStore spriteStore)
        {
            spriteManager.SpriteStore = spriteStore;
        }

        public void RegisterImageExclusionFilter(Predicate<BackgroundImageClass> imageExclusionFilter)
        {
            spriteManager.ImageExclusionFilter = imageExclusionFilter;
        }
    }
}
