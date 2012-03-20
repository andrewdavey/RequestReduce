using System.Web;
using Spritastic.Utilities;

namespace Spritastic
{
    public interface ISpritingSettings
    {
        int SpriteSizeLimit { get; set; }
        int SpriteColorLimit { get; set; }
        bool ImageQuantizationDisabled { get; set; }
        bool ImageOptimizationDisabled { get; set; }
        int ImageOptimizationCompressionLevel { get; set; }
        bool IsFullTrust { get; }
    }

    public class SpritingSettings : ISpritingSettings
    {
        public int SpriteSizeLimit { get; set; }
        public int SpriteColorLimit { get; set; }
        public bool ImageQuantizationDisabled { get; set; }
        public bool ImageOptimizationDisabled { get; set; }
        public int ImageOptimizationCompressionLevel { get; set; }
        public bool IsFullTrust
        {
            get { return TrustLevelChecker.GetCurrentTrustLevel() == AspNetHostingPermissionLevel.Unrestricted; }
        }
    }
}