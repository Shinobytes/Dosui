using System;
using System.Drawing;
using System.Linq;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class MenuStrip : Control
    {
        public ControlCollection<MenuItem> Items { get; }

        public MenuStrip()
        {
            Items = new ControlCollection<MenuItem>(this);
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = ConsoleColor.Gray;
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            // menustrips are abit special, it will always render full width of the parent
            graphics.DrawLine(Position.X, Position.Y, Parent.Size.Width, Position.Y, BackgroundColor);

            var totalWidth = 0;
            for (var index = 0; index < Items.Count; index++)
            {
                var menu = Items[index];

                // a menu item size is based on the text content
                menu.Position = new Point(Position.X + 1 + totalWidth, Position.Y);
                menu.Draw(graphics, appTime);

                totalWidth += (menu.Text.Count(x => x != '&')) + 2;
            }
        }

        public override void Update(AppTime appTime)
        {
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            this.Items.Where(x => x.HasKeyboardFocus && !x.EventBlocked()).DoWhile(x => x.OnKeyDown(key));
            return true;
        }
    }
}