using System;

namespace Shinobytes.Console.Forms.Graphics
{
    public class Viewport : IGraphics, IDisposable
    {
        private readonly IGraphics graphics;
        private int xOffset;
        private int yOffset;

        public Viewport(IGraphics graphics, int x, int y)
        {
            xOffset = x;
            yOffset = y;
            this.graphics = graphics;
        }


        public int Width => graphics.Width - xOffset;

        public int Height => graphics.Height - yOffset;

        public CharInfo[] Buffer => graphics.Buffer; // todo: fix, this is currently returning whole buffer

        public void Clear(ConsoleColor color = ConsoleColor.Black)
        {
            graphics.Clear(xOffset, yOffset, Width, Height, color);
        }

        public void Clear(int x, int y, int w, int h, ConsoleColor color)
        {
            graphics.Clear(xOffset + x, yOffset + y, w, h, color);
        }

        public char GetPixelChar(int x, int y)
        {
            return graphics.GetPixelChar(xOffset + x, yOffset + y);
        }

        public void SetPixelChar(char c, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            graphics.SetPixelChar(c, xOffset + x, yOffset + y, foreground, background);
        }

        public void SetPixelAsciiChar(char c, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            graphics.SetPixelAsciiChar(c, xOffset + x, yOffset + y, foreground, background);
        }

        public void SetPixel(int x, int y, ConsoleColor color)
        {
            graphics.SetPixel(xOffset + x, yOffset + y, color);
        }

        public void SetPixels(ConsoleColor[] pixels)
        {
            throw new NotImplementedException("SetPixels cannot be used in a Viewport as it would replace all pixels");

            graphics.SetPixels(pixels);
        }

        public void DrawLine(int x1, int y1, int x2, int y2, ConsoleColor color)
        {
            graphics.DrawLine(xOffset + x1, yOffset + y1, xOffset + x2, yOffset + y2, color);
        }

        public void DrawLineChar(char c, int x1, int y1, int x2, int y2, ConsoleColor color, ConsoleColor background)
        {
            graphics.DrawLineChar(c, xOffset + x1, yOffset + y1, xOffset + x2, yOffset + y2, color, background);
        }

        public void DrawSprite(ConsoleSprite sprite, int x, int y, bool flipHorizontal = false)
        {
            graphics.DrawSprite(sprite, xOffset + x, yOffset + y, flipHorizontal);
        }

        public void DrawString(string text, int x, int y, ConsoleColor foregroundColor, ConsoleColor background)
        {
            graphics.DrawString(text, xOffset + x, yOffset + y, foregroundColor, background);
        }

        public Viewport CreateViewport(int xOffset, int yOffset)
        {
            return new Viewport(this, this.xOffset + xOffset, this.yOffset + yOffset);
        }

        public void DrawRect(int x, int y, int width, int height, ConsoleColor backgroundColor)
        {
            this.graphics.DrawRect(xOffset + x, yOffset + y, width, height, backgroundColor);
        }

        public void DrawShadowRect(int x, int y, int width, int height, ConsoleColor backgroundColor)
        {
            this.graphics.DrawShadowRect(xOffset + x, yOffset + y, width, height, backgroundColor);
        }

        public void DrawBorder(BorderThickness thickness, int x, int y, int width, int height, ConsoleColor borderColor, ConsoleColor backgroundColor)
        {
            this.graphics.DrawBorder(thickness, xOffset + x, yOffset + y, width, height, borderColor, backgroundColor);
        }

        public ConsoleColor GetForeground(int x, int y)
        {
            return this.graphics.GetForeground(x + this.xOffset, y + yOffset);
        }

        public ConsoleColor GetBackground(int x, int y)
        {
            return this.graphics.GetBackground(x + this.xOffset, y + yOffset);
        }

        public void Dispose()
        {
        }

    }
}