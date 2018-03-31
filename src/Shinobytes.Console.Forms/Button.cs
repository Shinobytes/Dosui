using System;
using System.Drawing;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class Button : Control
    {
        public event EventHandler Invoke;

        public Button()
        {
            this.Size = new Size(0, 1);
            this.BackgroundColor = ConsoleColor.Gray;
            this.ForegroundColor = ConsoleColor.Black;
        }

        public bool DropShadow { get; set; } = true;

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            var size = new Size(Math.Max(this.RenderText.Length, this.Size.Width), this.Size.Height);
            if (DropShadow)
            {
                graphics.DrawShadowRect(this.Position.X, this.Position.Y, size.Width + 1, size.Height + 1,
                    this.HasFocus ? Application.ThemeColor : this.BackgroundColor);
            }
            else
            {
                graphics.DrawRect(this.Position.X, this.Position.Y, size.Width, size.Height,
                    this.HasFocus ? Application.ThemeColor : this.BackgroundColor);
            }

            var totWidth = 0;
            foreach (var op in this.TextRenderOperations)
            {
                graphics.DrawString(op.Text,
                    Position.X + (size.Width / 2 - RenderText.Length / 2) + totWidth,
                    Position.Y,
                    this.HasFocus ? ConsoleColor.White : op.ForegroundColor,
                    this.HasFocus ? Application.ThemeColor : this.BackgroundColor);
                totWidth += op.Text.Length;
            }

            //graphics.DrawString(
            //    Text,
            //    Position.X + (size.Width / 2 - RenderText.Length / 2),
            //    Position.Y,
            //    this.HasFocus ? ConsoleColor.White : this.ForegroundColor,
            //    this.HasFocus ? Application.ThemeColor : this.BackgroundColor);


            if (this.HasFocus)
            {
                graphics.SetPixel(
                    AsciiCodes.TriangleRight,
                    Position.X,
                    Position.Y,
                    ConsoleColor.White,
                    Application.ThemeColor);

                graphics.SetPixel(
                    AsciiCodes.TriangleLeft,
                    Position.X + size.Width - 1,
                    Position.Y,
                    ConsoleColor.White,
                    Application.ThemeColor);
            }

        }

        public override void Update(AppTime appTime)
        {
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                Invoke?.Invoke(this, EventArgs.Empty);
                return false;
            }

            return base.HandleNavigationKeys(key);
        }

        //public override string RenderText => this.HasFocus ? AsciiCodes.TriangleRight + $" {this.Text} " + AsciiCodes.TriangleLeft : this.Text;
    }
}