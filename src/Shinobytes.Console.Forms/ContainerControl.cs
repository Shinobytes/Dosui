using System;
using System.Drawing;
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

        public override bool ShortcutListener
        {
            get
            {
                if (base.ShortcutListener)
                {
                    return true;
                }

                return this.Controls.Count > 0 && this.Controls.Any(x => x.ShortcutListener);
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

        public override bool Navigation(Control sender, ConsoleKey key)
        {
            if (key == ConsoleKey.Escape)
                return true;

            int Distance(Point a, Point b) => (a.X - b.X) + (a.Y - b.Y);
            if (this.Controls.Count == 0) return true;
            var controls = this.Controls
                .Where(x => x != sender && x.CanFocus)
                .OrderBy(x => Distance(x.Position, sender.Position))
                .ToArray();

            if (controls.Length == 0) return true;
            var dir = -1;
            Control target = null;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    target = controls.OrderByDescending(x => x.Position.Y).FirstOrDefault(x => x.Position.Y < sender.Position.Y);
                    break;
                case ConsoleKey.DownArrow:
                    target = controls.OrderBy(x => x.Position.Y).FirstOrDefault(x => x.Position.Y > sender.Position.Y);
                    dir = 1;
                    break;
                case ConsoleKey.LeftArrow:
                    target = controls.OrderByDescending(x => x.Position.X).FirstOrDefault(x => x.Position.X < sender.Position.X);
                    break;
                case ConsoleKey.RightArrow:
                    target = controls.OrderBy(x => x.Position.X).FirstOrDefault(x => x.Position.X > sender.Position.X);
                    dir = 1;
                    break;
            }

            if (target == null && key != ConsoleKey.DownArrow && key != ConsoleKey.RightArrow) // down or right arrow should never go back up.
            {
                target = controls
                    .FirstOrDefault(x =>
                        dir > 0 ? x.TabIndex > sender.TabIndex : x.TabIndex < sender.TabIndex);
            }

            if (target)
            {
                target.Focus();
                return false;
            }
            return true;
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            // input movement
            if (key.Key == ConsoleKey.Tab)
            {
                // change focus to next item
                var activeControl = ActiveControl;
                var nextItem = this.Controls.OrderBy(x => x.TabIndex).FirstOrDefault(x => x.CanFocus && (activeControl == null || activeControl is MenuStrip || x.TabIndex > activeControl.TabIndex));
                if (nextItem != null)
                {
                    nextItem.Focus();
                    return false;
                }
            }
            else
            {
                var activeControl = this.ActiveControl;
                if (activeControl)
                {
                    if (activeControl.IsEnabled)
                    {
                        // unless this is an input control, we should listen for possible menu shortcuts
                        // and if one is reached, jump to it.
                        if (!IsNavigationOrActionKey(key.Key))
                        {
                            if (!Controls.Where(x => x.ShortcutListener).DoWhile(x => x.OnKeyDown(key)))
                            {
                                return false;
                            }
                        }

                        return activeControl.OnKeyDown(key);
                    }
                }

                Controls.Where(x => x.ShortcutListener || x.HasFocus && !x.EventBlocked())
                    .DoWhile(x => x.OnKeyDown(key));

                // navigate to first best control if no control has focus
                if (this is Window window)
                {
                    var hasFocusOrNoActiveControl = window.HasFocus || !this.ActiveControl;
                    var isArrowKey = IsArrowKey(key.Key);
                    var noActiveWindow = InputManager.ActiveWindow == null || InputManager.ActiveWindow == window;
                    if (isArrowKey && hasFocusOrNoActiveControl && noActiveWindow)
                    {
                        this.Focus();
                        return false;
                    }
                }
            }

            return true;
        }
    }
}