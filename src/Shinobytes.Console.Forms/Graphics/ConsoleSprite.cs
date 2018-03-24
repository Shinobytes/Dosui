using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Shinobytes.Console.Forms.Graphics
{
    public class ConsoleSprite
    {
        private readonly ConsoleImage consoleImage;
        private ConsoleColor?[] spritePixelCache;
        private ColorPalette colorPalette;

        public ConsoleSprite(ConsoleImage consoleImage)
        {
            this.consoleImage = consoleImage;
            Width = consoleImage.Width;
            Height = consoleImage.Height;
            colorPalette = consoleImage.GetColorPalette();
        }

        public int Width { get; }

        public int Height { get; }

        public ColorPalette Palette => colorPalette;

        public ConsoleColor?[] GetPixels(Rectangle? source = null)
        {
            ConsoleColor? Convert(Color c)
            {
                if (c.A == 0) // & c.R == 0 && c.G == 0 && c.B == 0
                {
                    return null;
                }
                
                return (ConsoleColor)Array.IndexOf(colorPalette.Entries, c);

                /* var index = c.R > 128 | c.G > 128 | c.B > 128 ? 8 : 0; // Bright bit
                index |= c.R > 64 ? 4 : 0; // Red bit
                index |= c.G > 64 ? 2 : 0; // Green bit
                index |= c.B > 64 ? 1 : 0; // Blue bit
                return (ConsoleColor)index;*/
            }

            if (source != null)
            {
                colorPalette = consoleImage.GetColorPalette();
                return consoleImage.GetPixels(source).Select(Convert).ToArray();
            }

            return spritePixelCache ?? (spritePixelCache = consoleImage.GetPixels().Select(Convert).ToArray());
        }
    }
}