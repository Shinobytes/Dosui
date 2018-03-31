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
        void SetPixel(char c, int x, int y, ConsoleColor foreground, ConsoleColor background);
        void SetPixelAscii(char c, int x, int y, ConsoleColor foreground, ConsoleColor background);
        void SetPixel(int x, int y, ConsoleColor color);
        void SetPixels(ConsoleColor[] pixels);
        void DrawLine(int x1, int y1, int x2, int y2, ConsoleColor color);
        void DrawLine(char c, int x1, int y1, int x2, int y2, ConsoleColor color, ConsoleColor background);
        void DrawSprite(ConsoleSprite sprite, int x, int y, bool flipHorizontal = false);
        void DrawString(string text, int x, int y, ConsoleColor foregroundColor, ConsoleColor background);
        Viewport CreateViewport(int xOffset, int yOffset);

        void DrawRect(int x, int y, int width, int height, ConsoleColor backgroundColor);
        void DrawShadowRect(int x, int y, int width, int height, ConsoleColor backgroundColor);
        void DrawBorder(BorderThickness thickness, int x, int y, int width, int height, ConsoleColor borderColor, ConsoleColor backgroundColor);
        void DrawBorder(BorderThickness thickness, int x, int y, int width, int height, 
            ConsoleColor borderTopColor, ConsoleColor borderRightColor, ConsoleColor borderBottomColor, ConsoleColor borderLeftColor, ConsoleColor backgroundColor);

        ConsoleColor GetForeground(int x, int y);
        ConsoleColor GetBackground(int x, int y);
    }
}