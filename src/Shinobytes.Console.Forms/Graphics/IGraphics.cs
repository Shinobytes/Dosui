using System;

namespace Shinobytes.Console.Forms.Graphics
{
    public interface IGraphics
    {
        int Width { get; }
        int Height { get; }
        CharInfo[] Buffer { get; }

        void Clear(ConsoleColor color = ConsoleColor.Black);
        void Clear(int x, int y, int w, int h, ConsoleColor color);
        char GetPixelChar(int x, int y);
        void SetPixelChar(char c, int x, int y, ConsoleColor foreground, ConsoleColor background);
        void SetPixelAsciiChar(char c, int x, int y, ConsoleColor foreground, ConsoleColor background);
        void SetPixel(int x, int y, ConsoleColor color);
        void SetPixels(ConsoleColor[] pixels);
        void DrawLine(int x1, int y1, int x2, int y2, ConsoleColor color);
        void DrawLineChar(char c, int x1, int y1, int x2, int y2, ConsoleColor color, ConsoleColor background);
        void DrawSprite(ConsoleSprite sprite, int x, int y, bool flipHorizontal = false);
        void DrawString(string text, int x, int y, ConsoleColor foregroundColor, ConsoleColor background);
        Viewport CreateViewport(int xOffset, int yOffset);

        void DrawRect(int x, int y, int width, int height, ConsoleColor backgroundColor);
        void DrawShadowRect(int x, int y, int width, int height, ConsoleColor backgroundColor);
        void DrawBorder(BorderThickness thickness, int x, int y, int width, int height, ConsoleColor borderColor, ConsoleColor backgroundColor);

        ConsoleColor GetForeground(int x, int y);
        ConsoleColor GetBackground(int x, int y);
    }

    public struct BorderThickness
    {
        public readonly int Top;
        public readonly int Left;
        public readonly int Right;
        public readonly int Bottom;
        public BorderThickness(int top, int left, int right, int bottom)
        {
            this.Top = top;
            this.Left = left;
            this.Right = right;
            this.Bottom = bottom;
        }

        public int Size => Top + Left + Right + Bottom;
    }
}