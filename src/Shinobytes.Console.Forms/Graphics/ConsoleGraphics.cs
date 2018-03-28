using System;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;

namespace Shinobytes.Console.Forms.Graphics
{
    public struct ConsolePixel
    {
        public ConsolePixel(char c, ConsoleColor background, ConsoleColor foreground)
        {
            Char = c;
            Background = background;
            Foreground = foreground;
        }

        public char Char { get; set; }
        public ConsoleColor Background { get; set; }
        public ConsoleColor Foreground { get; set; }
    }

    public class ConsoleGraphics : IGraphics
    {
        private ConsolePixel[] pixels;

        public ConsoleGraphics(int width, int height)
        {
            Resize(width, height);
        }

        public void Resize(int newWidth, int newHeight)
        {
            Width = newWidth;
            Height = newHeight;
            pixels = new ConsolePixel[newWidth * newHeight];
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public CharInfo[] Buffer
        {
            get
            {
                CharInfo[] buffer = new CharInfo[pixels.Length];
                for (var i = 0; i < pixels.Length; i++)
                {
                    buffer[i] = new CharInfo
                    {
                        UnicodeChar = pixels[i].Char,
                        Attributes = (short)((byte)pixels[i].Foreground | ((byte)pixels[i].Background << 4))
                    };
                }

                return buffer;
            }
        }

        /// <summary>
        ///     Gets or sets the active color palette used for drawing the next frame. Only one palette can be active per frame and can max use 16 colors.
        /// </summary>
        public ColorPalette Palette { get; set; }

        public void Clear(ConsoleColor color = ConsoleColor.Black)
        {
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i].Char = ' '; // █
                pixels[i].Background = color;
                pixels[i].Foreground = color;
                //pixels[i].Attributes = (short)((byte)color | ((byte)color << 4));// (byte)ConsoleColor.Red | (byte)ConsoleColor.Green << 4;
            }
        }

        public void Clear(int x, int y, int w, int h, ConsoleColor color)
        {
            for (var top = y; top < y + h; top++)
            {
                for (var left = x; left < x + w; left++)
                {
                    SetPixel(left, top, color);
                }
            }
        }

        public char GetPixelChar(int x, int y)
        {
            var i = y * Width + x;
            if (i >= pixels.Length) return ' ';
            return pixels[i].Char;
            //if (i >= Buffer.Length) return ' ';            
            //return Buffer[i].UnicodeChar;
        }

        public void SetPixel(int x, int y, ConsoleColor color)
        {
            var i = y * Width + x;
            if (i >= pixels.Length) return;
            pixels[i].Char = ' '; // █
            pixels[i].Background = color;
            pixels[i].Foreground = color;
            //Buffer[i].UnicodeChar = ' '; // █
            //Buffer[i].Attributes = (short)((byte)color | ((byte)color << 4));
        }

        public void SetPixelChar(char c, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            var i = y * Width + x;
            if (i >= pixels.Length || c == '\0') return;
            pixels[i].Char = c;
            pixels[i].Background = background;
            pixels[i].Foreground = foreground;
            //Buffer[i].UnicodeChar = c;            
            //Buffer[i].Attributes = (short)((byte)foreground | ((byte)background << 4));
        }

        public void SetPixelAsciiChar(char c, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            var i = y * Width + x;
            if (i >= pixels.Length || c == '\0') return;
            pixels[i].Char = c;
            pixels[i].Background = background;
            pixels[i].Foreground = foreground;

            //Buffer[i].bAsciiChar = (byte)c;
            //Buffer[i].Attributes = (short)((byte)foreground | ((byte)background << 4));
        }

        public void SetPixels(ConsoleColor[] pixels)
        {
            if (pixels.Length != Width * Height)
                throw new ArgumentException("Invalid amount of pixels supplied. Must be Width * Height.",
                    nameof(pixels));

            for (var i = 0; i < Buffer.Length; i++)
            {
                this.pixels[i].Background = pixels[i];
                this.pixels[i].Foreground = pixels[i];
                //Buffer[i].Attributes = (short)((byte)pixels[i] | ((byte)pixels[i] << 4));
            }
        }

        public void DrawLine(int x1, int y1, int x2, int y2, ConsoleColor color)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            for (var x = x1; x < x2; x++)
            {
                var y = y1 + dy * (x - x1) / dx;
                SetPixel(x, y, color);
            }
        }

        public void DrawLineChar(char c, int x1, int y1, int x2, int y2, ConsoleColor color, ConsoleColor background)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            for (var x = x1; x < x2; x++)
            {
                var y = y1 + dy * (x - x1) / dx;
                SetPixelChar(c, x, y, color, background);
            }
        }

        // just for lols, not really necessary :P stupid really. But you gotta enjoy your coding!
        public static implicit operator CharInfo[] (ConsoleGraphics graphics)
        {
            return graphics.Buffer;
        }

        public void DrawString(string text, int x, int y, ConsoleColor foregroundColor, ConsoleColor background)
        {
            for (var i = 0; i < text.Length; i++)
            {
                SetPixelChar(text[i], x + i, y, foregroundColor, background);
            }
        }

        public void DrawSprite(ConsoleSprite sprite, int x, int y, bool flipHorizontal = false)
        {
            var pixels = sprite.GetPixels();
            for (var y1 = 0; y1 < sprite.Height; ++y1)
            {
                for (var x1 = 0; x1 < sprite.Width; ++x1)
                {
                    var spi = flipHorizontal
                        ? y1 * sprite.Width + (sprite.Width - 1 - x1)
                        : y1 * sprite.Width + x1;

                    if (spi >= pixels.Length) return;
                    if (pixels[spi] == null) continue;
                    var color = pixels[spi].Value;
                    var i = (y + y1) * Width + (x + x1);
                    if (i >= this.pixels.Length) return;

                    this.pixels[i].Background = color;
                    this.pixels[i].Foreground = color;
                    //Buffer[i].Attributes = (short)((byte)color | ((byte)color << 4));
                }
            }
        }

        public Viewport CreateViewport(int x, int y)
        {
            return new Viewport(this, x, y);
        }

        public void DrawRect(int x, int y, int width, int height, ConsoleColor backgroundColor)
        {
            for (var y0 = y; y0 < y + height; y0++)
            {
                for (var x0 = x; x0 < x + width; x0++)
                {
                    this.SetPixel(x0, y0, backgroundColor);
                }
            }
        }

        public void DrawShadowRect(int x, int y, int width, int height, ConsoleColor backgroundColor)
        {
            DrawRect(x, y, width - 1, height - 1, backgroundColor);

            var narrowShadow = height < 3;
            var shadowRight = AsciiCodes.GetAsciiChar(220);
            var shadowBottom = AsciiCodes.GetAsciiChar(223);
            for (var i = y; i < (y + height); i++)
            {

                if (narrowShadow)
                {
                    // most likely a button, or just a very narrow control
                    // unfortunately, the only way to get a proper dropshadow for such
                    // control is to use the half-size block ▄
                    // and this hides the text under it.                    
                    var bg0 = this.GetBackground(x + width - 1, i);

                    SetPixelChar(i == y 
                            ? shadowRight 
                            : shadowBottom, x + width - 1, i, ConsoleColor.Black, bg0);
                }
                else
                {
                    var c0 = this.GetPixelChar(x + width - 1, i);
                    var c1 = this.GetPixelChar(x + width, i);
                    SetPixelChar(c0, x + width - 1, i, ConsoleColor.DarkGray, ConsoleColor.Black);
                    SetPixelChar(c1, x + width, i, ConsoleColor.DarkGray, ConsoleColor.Black);
                }
            }

            for (var i = x + 1; i < (x + width - 1); i++)
            {
                if (narrowShadow)
                {
                    var sampledBg = this.GetBackground(i, y + height - 1);
                    SetPixelChar(shadowBottom, i, y + height - 1, ConsoleColor.Black, sampledBg);
                }
                else
                {
                    var sampledChar = this.GetPixelChar(i, y + height - 1);
                    SetPixelChar(sampledChar, i, y + height - 1, ConsoleColor.DarkGray, ConsoleColor.Black);
                }
            }
        }

        public void DrawBorder(BorderThickness thickness, int x, int y, int width, int height, ConsoleColor borderColor, ConsoleColor backgroundColor)
        {
            var topBorder = AsciiCodes.GetBorderChars(thickness.Top);
            var leftBorder = AsciiCodes.GetBorderChars(thickness.Left);
            var rightBorder = AsciiCodes.GetBorderChars(thickness.Right);
            var botBorder = AsciiCodes.GetBorderChars(thickness.Bottom);

            // draw vertical lines
            for (var i = 0; i < height - 1; i++)
            {
                SetPixelChar(leftBorder[3], x, y + i + 1, borderColor, backgroundColor);
                SetPixelChar(rightBorder[3], x + width, y + i + 1, borderColor, backgroundColor);
            }

            // draw top corners
            SetPixelChar(topBorder[0], x, y, borderColor, backgroundColor);
            SetPixelChar(topBorder[1], x + width, y, borderColor, backgroundColor);

            // draw horizontal lines
            for (var i = 0; i < width - 1; i++)
            {
                SetPixelChar(topBorder[2], x + i + 1, y, borderColor, backgroundColor);
                SetPixelChar(botBorder[2], x + i + 1, y + height, borderColor, backgroundColor);
            }

            // draw bottom corners
            SetPixelChar(botBorder[4], x, y + height, borderColor, backgroundColor);
            SetPixelChar(botBorder[5], x + width, y + height, borderColor, backgroundColor);
        }

        public ConsoleColor GetForeground(int x, int y)
        {
            var index = y * Width + x;
            if (index < 0 || index >= this.pixels.Length)
                return ConsoleColor.Black;

            return pixels[index].Foreground;
        }

        public ConsoleColor GetBackground(int x, int y)
        {
            var index = y * Width + x;
            if (index < 0 || index >= this.pixels.Length)
                return ConsoleColor.Black;

            return pixels[index].Background;
        }
    }
}