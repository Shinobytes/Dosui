using System;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class ToggleButton : Control
    {
        public bool IsChecked { get; set; }
        public event EventHandler Invoke;

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            var cb = IsChecked ? AsciiCodes.CheckBox_Checked : AsciiCodes.CheckBox_Unchecked;
            var bg = this.BackgroundColor;
            var fg = IsEnabled ? this.ForegroundColor : this.DisabledForegroundColor;

            if (this.HasFocus && this.IsEnabled)
            {
                bg = Application.ThemeColor;
                fg = ConsoleColor.White;
            }

            graphics.DrawString($"{cb} {Text}", this.Position.X, this.Position.Y, fg, bg);
        }

        public override void Update(AppTime appTime)
        {
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            if (this.HasFocus && key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                this.IsChecked = !this.IsChecked;
                Invoke?.Invoke(this, EventArgs.Empty);
                return false;
            }

            return base.HandleNavigationKeys(key);
        }
    }
}