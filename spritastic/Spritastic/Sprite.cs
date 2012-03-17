using System.Drawing;

namespace Spritastic
{
    public class Sprite
    {
        public Sprite(string url, Bitmap image)
        {
            Image = image;
            Url = url;
        }

        public string Url { get; private set; }
        public Bitmap Image { get; private set; }
    }
}