using System;

namespace Spritastic
{
    public class SpriteException : ApplicationException
    {
        public SpriteException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SpriteException(string cssRule, int cssContentOffset, string message, Exception innerException) : base(message, innerException)
        {
            CssContentOffset = cssContentOffset;
            CssRule = cssRule;
        }

        public string CssRule { get; private set; }
        public int CssContentOffset { get; private set; }
    }
}