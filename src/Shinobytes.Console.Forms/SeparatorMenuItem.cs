using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class SeparatorMenuItem : MenuItem
    {
        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            var isThin = true;// this.BorderThickness <= 1;

            graphics.DrawLineChar(isThin ? AsciiCodes.BorderSingle_Horizontal : AsciiCodes.BorderDouble_Horizontal,
                this.Position.X, this.Position.Y, this.Position.X + this.Size.Width - 1, this.Position.Y,
                this.BorderColor, this.BackgroundColor);

            graphics.SetPixelChar(isThin ? AsciiCodes.BorderSingle_SplitToRight : AsciiCodes.BorderDouble_SplitToRight,
                this.Position.X - 1, this.Position.Y, this.BorderColor, this.BackgroundColor);

            graphics.SetPixelChar(isThin ? AsciiCodes.BorderSingle_SplitToLeft : AsciiCodes.BorderDouble_SplitToLeft,
                this.Position.X + this.Size.Width - 1, this.Position.Y, this.BorderColor, this.BackgroundColor);
        }
    }
}