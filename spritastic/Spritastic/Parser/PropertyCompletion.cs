using System;

namespace Spritastic.Parser
{
    [Flags]
    public enum PropertyCompletion
    {
        HasNothing = 0,
        HasImage = 1,
        HasRepeat = 2,
        HasXOffset = 4,
        HasYOffset = 8,
        HasWidth = 16,
        HasHeight = 32,
        HasPaddingLeft = 64,
        HasPaddingRight = 128,
        HasPaddingTop = 256,
        HasPaddingBottom = 512
    }
}