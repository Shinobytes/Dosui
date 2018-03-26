using System;
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

        public Control ActiveControl
        {
            get => this.Controls.FirstOrDefault(x => x.HasFocus);
            set
            {
                this.ActiveControl?.Blur();
                value?.Focus();
            }
        }

        public T ControlAt<T>(int index) where T : Control
        {
            return Controls.ElementAt<T>(index);
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            Controls.Where(x => x.Visible).ForEach(x => x.Draw(graphics, appTime));
        }

        public override void Update(AppTime appTime)
        {
            Controls.Where(x => x.IsEnabled).ForEach(x => x.Update(appTime));
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            // input movement


            if (key.Key == ConsoleKey.Tab)
            {
                // change focus to next item
                var ac = InputManager.ActiveControl;
                var activeControl = ActiveControl;
                var nextItem = this.Controls.OrderBy(x => x.TabIndex).FirstOrDefault(x => x.CanFocus && (activeControl == null || x.TabIndex > activeControl.TabIndex));
                if (nextItem != null)
                {
                    nextItem.Focus();
                    return false;
                }
            }
            else
            {



                var activeControl = this.ActiveControl;
                if (activeControl != null)
                {
                    if (activeControl.IsEnabled)
                    {

                        // unless this is an input control, we should listen for possible menu shortcuts
                        // and if one is reached, jump to it.
                        if (!IsNavigationKey(key.Key))
                        {
                            if (!Controls.Where(x => x.ShortcutListener).DoWhile(x => x.OnKeyDown(key)))
                            {
                                return false;
                            }
                        }


                        return activeControl.OnKeyDown(key);
                    }
                }


                Controls.Where(x => x.ShortcutListener || x.HasFocus && !x.EventBlocked()).DoWhile(x => x.OnKeyDown(key));
            }

            return true;
        }

        private static bool IsNavigationKey(ConsoleKey key)
        {
            return key == ConsoleKey.Enter || key == ConsoleKey.Escape || key == ConsoleKey.LeftArrow ||
                   key == ConsoleKey.UpArrow || key == ConsoleKey.RightArrow || key == ConsoleKey.DownArrow;
        }
    }
}