using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Spritastic.Parser;
using Spritastic.Utilities;

namespace Spritastic.Generator
{
    public class SpriteManager : ISpriteManager
    {
        ISpriteContainer spriteContainer;
        readonly SpritingSettings config;
        readonly Func<byte[], string> saveSpriteAndReturnUrl;
        readonly Func<ISpriteContainer> createNewSpriteContainer;
        readonly IPngOptimizer pngOptimizer;
        readonly IEnumerable<Func<BackgroundImageClass, bool>> imageExclusions;
        readonly IList<KeyValuePair<ImageMetadata, SpritedImage>> spriteList = new List<KeyValuePair<ImageMetadata, SpritedImage>>();

        public SpriteManager(SpritingSettings config, Func<ISpriteContainer> createNewSpriteContainer, Func<byte[], string> saveSpriteAndReturnUrl, IPngOptimizer pngOptimizer, IEnumerable<Func<BackgroundImageClass, bool>> imageExclusions)
        {
            this.createNewSpriteContainer = createNewSpriteContainer;
            this.pngOptimizer = pngOptimizer;
            this.imageExclusions = imageExclusions;
            this.config = config;
            this.saveSpriteAndReturnUrl = saveSpriteAndReturnUrl;
            spriteContainer = createNewSpriteContainer();
            Errors = new List<Exception>();
        }

        // For testing access
        internal IList<KeyValuePair<ImageMetadata, SpritedImage>> SpriteList
        {
            get { return spriteList; }
        }

        public IList<Exception> Errors { get; internal set; }

        public virtual void Add(BackgroundImageClass image)
        {
            if (imageExclusions.Any(exclude => exclude(image))) return;

            var imageKey = new ImageMetadata(image);
            
            if (spriteList.Any(x => x.Key.Equals(imageKey)))
            {
                var originalImage = spriteList.First(x => x.Key.Equals(imageKey)).Value;
                var clonedImage = new SpritedImage(originalImage.AverageColor, image, originalImage.Image) { Position = originalImage.Position, Url = originalImage.Url, Metadata = imageKey };
                spriteList.Add(new KeyValuePair<ImageMetadata, SpritedImage>(imageKey, clonedImage));
                return;
            }
            SpritedImage spritedImage;
            var sprite = spriteList.FirstOrDefault(x => x.Value.CssClass.ImageUrl == image.ImageUrl);
            if(sprite.Value != null)
            {
                image.IsSprite = true;
                sprite.Value.CssClass.IsSprite = true;
            }
            try
            {
                spritedImage = spriteContainer.AddImage(image);
                spritedImage.Metadata = imageKey;
            }
            catch (Exception ex)
            {
                var message = string.Format("There were errors reducing {0}", image.ImageUrl);
                Tracer.Trace(message);
                Tracer.Trace(ex.ToString());
                var wrappedException = new ApplicationException(message, ex);
                Errors.Add(wrappedException);
                return;
            }
            spriteList.Add(new KeyValuePair<ImageMetadata, SpritedImage>(imageKey, spritedImage));
            if (spriteContainer.Size >= config.SpriteSizeLimit || (spriteContainer.Colors >= config.SpriteColorLimit && !config.ImageQuantizationDisabled && !config.ImageOptimizationDisabled))
                Flush();
        }

        public virtual void Flush()
        {
            if(spriteContainer.Size > 0)
            {
                Tracer.Trace("Beginning to Flush sprite");
                using (var spriteWriter = new SpriteWriter(spriteContainer.Width, spriteContainer.Height))
                {
                    var offset = 0;
                    foreach (var image in spriteContainer)
                    {
                        spriteWriter.WriteImage(image.Image);
                        image.Position = offset;
                        offset += image.Image.Width + 1;
                    }
                    var bytes = spriteWriter.GetBytes("image/png");
                    byte[] optBytes;
                    try
                    {
                        optBytes = (config.ImageOptimizationDisabled || !config.IsFullTrust) ? bytes : pngOptimizer.OptimizePng(bytes, config.ImageOptimizationCompressionLevel, config.ImageQuantizationDisabled);
                    }
                    catch (OptimizationException optEx)
                    {
                        optBytes = bytes;
                        Tracer.Trace(string.Format("Errors optimizing. Received Error: {0}", optEx.Message));
                        Errors.Add(optEx);
                    }
                    var url = saveSpriteAndReturnUrl(optBytes);
                    foreach (var image in spriteContainer)
                    {
                        image.Url = url;
                        foreach (var dupImage in spriteList)
                        {
                            if (dupImage.Key.Equals(image.Metadata) && dupImage.Value.Position == -1)
                            {
                                dupImage.Value.Position = image.Position;
                                dupImage.Value.Url = image.Url;
                            }
                        }
                    }
                }
                Tracer.Trace("Finished Flushing sprite");
            }
            spriteContainer = createNewSpriteContainer();
        }

        public IEnumerator<SpritedImage> GetEnumerator()
        {
            return spriteList.Select(x => x.Value).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            Flush();
            spriteList.ToList().ForEach(x => x.Value.Image.Dispose());
        }

        public struct ImageMetadata
        {
            public ImageMetadata(BackgroundImageClass image) : this()
            {
                Url = image.ImageUrl;
                Width = image.Width ?? 0;
                Height = image.Height ?? 0;
                XOffset = image.XOffset.Offset;
                YOffset = image.YOffset.Offset;
            }

            public int Width { get; set; }
            public int Height { get; set; }
            public int XOffset { get; set; }
            public int YOffset { get; set; }
            public string Url { get; set; }
        }
    }
}