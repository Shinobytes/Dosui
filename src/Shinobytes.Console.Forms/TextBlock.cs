using System;
using System.Drawing;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class TextBlock : Control
    {
        public TextBlock()
        {
            this.CanFocus = false;
        }

        public TextBlock(string text) : this()
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
            if (string.IsNullOrEmpty(this.Text)) return;
            var rows = this.Text.Split('\n');
            for (var i = 0; i < rows.Length; i++)
            {
                graphics.DrawString(rows[i], this.Position.X, this.Position.Y + i, this.ForegroundColor, this.BackgroundColor);
            }
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