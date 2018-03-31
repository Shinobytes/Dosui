using System;
using System.Drawing;
using System.Linq;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class StatusStrip : ContainerControl
    {
        public StatusStrip()
        {
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = ConsoleColor.Gray;
            CanFocus = false;
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            graphics.DrawRect(this.Position.X, this.Position.Y, this.Size.Width, this.Size.Height, this.BackgroundColor);
            var totalWidth = 0;
            for (var index = 0; index < Controls.Count; index++)
            {
                var item = Controls[index];
                item.Position = new Point(Position.X + 1 + totalWidth, Position.Y);
                item.Draw(graphics, appTime);
                totalWidth += (item.Text.Count(x => x != '&')) + 2;
            }
        }

        public override void Update(AppTime appTime)
        {
            this.Size = new Size(Parent.Size.Width, 1);
            this.Position = new Point(0, Parent.Size.Height - 1);            
            base.Update(appTime);
        }
    }
}