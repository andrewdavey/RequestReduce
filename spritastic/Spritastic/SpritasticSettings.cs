namespace Spritastic
{
    public class SpritingSettings
    {
        public int SpriteSizeLimit { get; set; }
        public int SpriteColorLimit { get; set; }
        public bool ImageQuantizationDisabled { get; set; }
        public bool ImageOptimizationDisabled { get; set; }
        public int ImageOptimizationCompressionLevel { get; set; }
        public bool IsFullTrust { get; set; }
    }
}