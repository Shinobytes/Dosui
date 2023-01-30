using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class Image : Control
    {
        private ConsoleSprite sprite;
        private ConsoleImage imageSource;

        public Image()
        {
            this.CanFocus = false;
        }

        public Image(ConsoleImage image) : this()
        {
            ImageSource = image;
        }

        public ConsoleImage ImageSource
        {
            get => imageSource;
            set
            {
                imageSource = value;
                sprite = new ConsoleSprite(imageSource);
            }
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            if (ImageSource != null)
            {
                graphics.DrawSprite(sprite, Position.X, Position.Y);
            }
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            return true;
        }

        public override void Update(AppTime appTime)
        {
        }
    }
}