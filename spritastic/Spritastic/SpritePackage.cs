using System.Collections.Generic;
using Spritastic.Generator;

namespace Spritastic
{
    public class SpritePackage
    {
        public SpritePackage(string generatedCss, IList<Sprite> sprites, IList<SpriteException> exceptions)
        {
            Exceptions = exceptions;
            Sprites = sprites;
            GeneratedCss = generatedCss;
        }

        public string GeneratedCss { get; private set; }
        public IList<Sprite> Sprites { get; private set; }
        public IList<SpriteException> Exceptions { get; private set; }
    }
}