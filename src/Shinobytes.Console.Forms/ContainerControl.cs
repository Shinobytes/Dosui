using System.Linq;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public abstract class ContainerControl : Control
    {
        protected ContainerControl()
        {
            Controls = new ControlCollection(this);
        }

        public ControlCollection Controls { get; }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            Controls.Where(x => x.Visible).ForEach(x => x.Draw(graphics, appTime));
        }

        public override void Update(AppTime appTime)
        {
            Controls.Where(x => x.Enabled).ForEach(x => x.Update(appTime));
        }

        public override bool OnKeyDown(KeyInfo key)
        {
             Controls.Where(x => x.HasKeyboardFocus && !x.EventBlocked()).DoWhile(x => x.OnKeyDown(key));
            return true;
        }
    }
}