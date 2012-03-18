namespace Spritastic.SpriteStore
{
    public interface ISpriteStore
    {
        string SaveSpriteAndReturnUrl(byte[] spriteBytes);
    }
}
