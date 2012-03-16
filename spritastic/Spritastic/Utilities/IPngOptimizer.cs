namespace Spritastic.Utilities
{
    public interface IPngOptimizer
    {
        byte[] OptimizePng(byte[] bytes, int compressionLevel, bool imageQuantizationDisabled);
    }
}