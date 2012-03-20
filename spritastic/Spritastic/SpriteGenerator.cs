using System;
using System.Linq;
using System.Security.Cryptography;
using Spritastic.Generator;
using Spritastic.ImageLoad;
using Spritastic.Parser;
using Spritastic.Selector;
using Spritastic.Utilities;
using nQuant;

namespace Spritastic
{
    public class SpriteGenerator
    {
        [ThreadStatic] private static MD5CryptoServiceProvider md5;

        private static readonly ICssImageTransformer DefaultCssImageTransformer =
            new CssImageTransformer(new CssSelectorAnalyzer());

        private static readonly IImageLoader DefaultImageLoader;

        private readonly ICssImageTransformer cssImageTransformer;
        private readonly Func<IImageLoader, Func<byte[], string>, ISpriteManager> createSpriteManager;

        public SpriteGenerator(ISpritingSettings settings)
            : this(
                DefaultCssImageTransformer,
                (imageLoader, urlGenerator) =>
                new SpriteManager(settings, imageLoader, urlGenerator,
                                  new PngOptimizer(new FileWrapper(), new WuQuantizer())))
        {

        }

        internal SpriteGenerator(ICssImageTransformer cssImageTransformer,
                                 Func<IImageLoader, Func<byte[], string>, ISpriteManager> createSpriteManager)
        {
            this.cssImageTransformer = cssImageTransformer;
            this.createSpriteManager = createSpriteManager;
        }

        public SpritePackage GenerateFromCss(string cssContent)
        {
            var newImages = cssImageTransformer.ExtractImageUrls(cssContent);
            using (
                var spriteManager = createSpriteManager(ImageLoader ?? DefaultImageLoader,
                                                        UrlGenerator ?? DefaultUrlGenerator))
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

        public IImageLoader ImageLoader { get; set; }

        public Func<byte[], string> UrlGenerator { get; set; }

        internal string DefaultUrlGenerator(byte[] bytes)
        {
            if (md5 == null)
                md5 = new MD5CryptoServiceProvider();
            return string.Format("{0}-Spritastic.png", new Guid(md5.ComputeHash(bytes)));
        }

        public Predicate<BackgroundImageClass> ImageExclusionFilter { get; set; }
    }
}
