using System;
using Spritastic.Parser;

namespace Spritastic
{
    public class SpriteGenerator
    {
        private readonly SpritingSettings settings;
        private Func<string, byte[]> getImageBytes;
        private Func<byte[], string> saveSpriteAndReturnUrl;
        private Predicate<BackgroundImageClass> shouldExcludeImage;

        public SpriteGenerator(SpritingSettings settings)
        {
            this.settings = settings;
        }

        public SpritePackage GenerateFromCss(string cssContent)
        {
            return null;
        }

        public void RegisterImageLoader(Func<string, byte[]> getImageBytes)
        {
            this.getImageBytes = getImageBytes;
        }

        public void RegisterSpriteStorage(Func<byte[], string> saveSpriteAndReturnUrl)
        {
            this.saveSpriteAndReturnUrl = saveSpriteAndReturnUrl;
        }

        public void RegisterImageExclusionFilter(Predicate<BackgroundImageClass> shouldExcludeImage)
        {
            this.shouldExcludeImage = shouldExcludeImage;
        }
    }
}
