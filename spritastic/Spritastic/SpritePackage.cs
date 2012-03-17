using System.Collections.Generic;

namespace Spritastic
{
    public class SpritePackage
    {
        public SpritePackage(string generatedCss, IList<Sprite> sprites)
        {
            Sprites = sprites;
            GeneratedCss = generatedCss;
        }

        public string GeneratedCss { get; private set; }
        public IList<Sprite> Sprites { get; private set; }
        public IList<SpriteException> Exceptions { get; private set; }
    }
}