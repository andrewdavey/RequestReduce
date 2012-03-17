using System;
using Spritastic.Parser;

namespace Spritastic
{
    public class SpriteGenerator
    {
        private readonly SpritingSettings settings;
        private readonly Func<string, byte[]> getImageBytes;
        private readonly Func<byte[], string> saveSpriteAndReturnUrl;
        private readonly Predicate<BackgroundImageClass> shouldExcludeImage;

        public SpriteGenerator(SpritingSettings settings, Func<string, byte[]> getImageBytes, Func<byte[], string> saveSpriteAndReturnUrl)
            : this(settings, getImageBytes, saveSpriteAndReturnUrl, null)
        {
        }

        public SpriteGenerator(SpritingSettings settings, Func<string, byte[]> getImageBytes, Func<byte[], string> saveSpriteAndReturnUrl, Predicate<BackgroundImageClass> shouldExcludeImage)
        {
            this.settings = settings;
            this.getImageBytes = getImageBytes;
            this.saveSpriteAndReturnUrl = saveSpriteAndReturnUrl;
            this.shouldExcludeImage = shouldExcludeImage;
        }

        public SpritePackage GenerateFromCss(string cssContent)
        {
            return null;
        }
    }
}
