using System;
using System.Drawing;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class TextBlock : Control
    {
        public TextBlock()
        {
        }

        public TextBlock(string text)
        {
            this.Text = text;
        }

        public TextBlock(string text, ConsoleColor foregroundColor) : this(text)
        {
            this.ForegroundColor = foregroundColor;
        }
        public TextBlock(string text, Point position) : this(text)
        {
            this.Position = position;
        }
        public TextBlock(string text, ConsoleColor foregroundColor, Point position) : this(text, foregroundColor)
        {
            this.Position = position;
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            graphics.DrawString(this.Text, this.Position.X, this.Position.Y, this.ForegroundColor, this.BackgroundColor);
        }

        public override void Update(AppTime appTime)
        {
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            return true;
        }
    }
}