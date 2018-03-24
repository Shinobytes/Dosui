using System;
using System.Drawing;

namespace Shinobytes.Console.Forms.Graphics
{
    public class Spritesheet : ConsoleImage
    {
        public Spritesheet(Bitmap image, int spriteWidth, int spriteHeight) : base(image)
        {
            SpriteWidth = spriteWidth;
            SpriteHeight = spriteHeight;
        }

        public static Spritesheet FromFile(string fileName, int frameWidth, int frameHeight)
        {
            return new Spritesheet((Bitmap)System.Drawing.Image.FromFile(fileName), frameWidth, frameHeight);
        }

        public ConsoleImage GetSpriteExact(int x, int y, int w, int h)
        {
            if (x < 0 || x > Width ||
                y < 0 || y > Height) throw new IndexOutOfRangeException();

            // TODO: Cache this, best case would be to just generate all sprites when creating the spritesheet
            var pixels = GetPixels(new Rectangle(x, y, w, h));
            return new ConsoleImage(pixels, SpriteWidth, SpriteHeight);
        }

        public ConsoleImage GetSprite(int column, int row)
        {
            var columns = Width / SpriteWidth;
            var rows = Height / SpriteHeight;
            if (row < 0 || row > rows ||
                column < 0 || column > columns) throw new IndexOutOfRangeException();

            // TODO: Cache this, best case would be to just generate all sprites when creating the spritesheet
            var pixels = GetPixels(new Rectangle(column * SpriteWidth, row * SpriteHeight, SpriteWidth, SpriteHeight));
            return new ConsoleImage(pixels, SpriteWidth, SpriteHeight);
        }

        public int SpriteWidth { get; }
        public int SpriteHeight { get; }
    }
}