using System;

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
}