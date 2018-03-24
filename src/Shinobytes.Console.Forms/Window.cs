using System;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class Window : ContainerControl
    {
        public bool IsMainWindow { get; internal set; }
        public DialogResult DialogResult { get; set; }
        public ConsoleColor CaptionColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor BorderColor { get; set; } = ConsoleColor.DarkGray;
        public Window()
        {
            WindowManager.Register(this);

            Visible = false;
            BackgroundColor = ConsoleColor.DarkBlue;
            ForegroundColor = ConsoleColor.Black;
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            if (!Visible || Size.Width * Size.Height <= 0) return;
            if (IsMainWindow)
            {
                DrawWindowFullSize(graphics, appTime);
            }
            else
            {
                DrawWindow(graphics, appTime);
            }
        }

        private void DrawWindow(IGraphics graphics, AppTime appTime)
        {
            // window background
            graphics.Clear(Position.X, Position.Y, Size.Width, Size.Height, BackgroundColor);

            // window background shadow drop
            //  - right shadow

            var borderPaddingX = 1;

            for (var i = 0; i < Size.Height; i++)
            {
                // drop shadow, right side
                var c0 = graphics.GetPixelChar(Position.X + Size.Width, Position.Y + i + 1);
                var c1 = graphics.GetPixelChar(Position.X + Size.Width + 1, Position.Y + i + 1);

                // 1. get char at x,y
                // 2. set pixel char, same char but with black bg and gray foreground to create a "shadow" ontop of text.
                graphics.SetPixelChar(c0, Position.X + Size.Width, Position.Y + i + 1, ConsoleColor.DarkGray, ConsoleColor.Black);
                graphics.SetPixelChar(c1, Position.X + Size.Width + 1, Position.Y + i + 1, ConsoleColor.DarkGray, ConsoleColor.Black);

                // borders, both left and right
                if (i == 0 || i == Size.Height) continue;
                graphics.SetPixelChar(AsciiCodes.BorderSingle_Vertical, Position.X + borderPaddingX, Position.Y + i, this.BorderColor, this.BackgroundColor);
                graphics.SetPixelChar(AsciiCodes.BorderSingle_Vertical, Position.X + Size.Width - (1 + borderPaddingX), Position.Y + i, this.BorderColor, this.BackgroundColor);
            }


            // window caption background and border bottom
            for (var i = 0; i < Size.Width; i++)
            {
                // bottom shadow
                var c0 = graphics.GetPixelChar(Position.X + 1 + i, Position.Y + Size.Height);
                graphics.SetPixelChar(c0, Position.X + 1 + i, Position.Y + Size.Height, ConsoleColor.DarkGray, ConsoleColor.Black);

                // border bottom and top
                if (i <= borderPaddingX || i >= Size.Width - (1 + borderPaddingX)) continue;
                graphics.SetPixelChar(AsciiCodes.BorderSingle_Horizontal, Position.X + i, Position.Y, this.BorderColor, this.BackgroundColor);
                graphics.SetPixelChar(AsciiCodes.BorderSingle_Horizontal, Position.X + i, Position.Y + this.Size.Height - 1, this.BorderColor, this.BackgroundColor);
            }

            // draw top border corners
            graphics.SetPixelChar(AsciiCodes.BorderSingle_TopLeft, Position.X + borderPaddingX, Position.Y, this.BorderColor, this.BackgroundColor);
            graphics.SetPixelChar(AsciiCodes.BorderSingle_TopRight, Position.X - borderPaddingX + Size.Width - 1, Position.Y, this.BorderColor, this.BackgroundColor);

            // draw bottom border corners
            graphics.SetPixelChar(AsciiCodes.BorderSingle_BottomLeft, Position.X + borderPaddingX, Position.Y + this.Size.Height - 1, this.BorderColor, this.BackgroundColor);
            graphics.SetPixelChar(AsciiCodes.BorderSingle_BottomRight, Position.X - borderPaddingX + Size.Width - 1, Position.Y + this.Size.Height - 1, this.BorderColor, this.BackgroundColor);

            // window caption text
            graphics.DrawString(Text, Position.X + (Size.Width / 2 - Text.Length / 2) - 1, Position.Y, ForegroundColor, CaptionColor);

            using (var gfx = graphics.CreateViewport(this.Position.X + 2, this.Position.Y + 1))
            {
                // finally draw all child components last
                base.Draw(gfx, appTime);
            }
        }

        private void DrawWindowFullSize(IGraphics graphics, AppTime appTime)
        {
            // window background
            graphics.Clear(BackgroundColor);

            // draw children as is, since our container is taking up the whole application
            base.Draw(graphics, appTime);
        }

        public void Show()
        {
            Visible = true;
        }

        public void Close()
        {
            Visible = false;
            Enabled = false;
            WindowManager.Unregister(this);
        }

        public DialogResult ShowDialog()
        {
            return DialogResult;
        }

        public void Hide()
        {
            Visible = false;
        }
    }
}
