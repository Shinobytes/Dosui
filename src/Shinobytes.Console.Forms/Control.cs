using System;
using System.Drawing;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public abstract class Control
    {
        protected Control()
        {
        }

        protected Control(Control parent) : this()
        {
            Parent = parent;
        }

        protected Control(Control parent, string text) : this(parent)
        {
            Text = text;
        }

        public void Focus()
        {
            HasFocus = true;
            HasKeyboardFocus = true;
        }

        public void Blur()
        {
            HasFocus = false;
            HasKeyboardFocus = false;
        }
        private bool eventBlocked = false;
        public bool Visible { get; set; } = true;
        public bool Enabled { get; set; } = true;

        public bool HasFocus { get; private set; } = true;
        public bool HasKeyboardFocus { get; private set; } = true; // todo: do not have Focus and Keyboard Focus on as standard.

        public bool TransparentBackground { get; set; }
        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;
        public ConsoleColor DisabledForegroundColor { get; set; } = ConsoleColor.DarkGray;

        public Size Size { get; set; }
        public Point Position { get; set; }

        public string Text { get; set; }
        public Control Parent { get; internal set; }


        public abstract void Draw(IGraphics graphics, AppTime appTime);

        public abstract void Update(AppTime appTime);

        /// <summary>
        /// Return true to swallow the keydown to prevent further controls to get the keypress
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract bool OnKeyDown(KeyInfo key);

        public void StopNextEvent()
        {
            eventBlocked = true;
        }

        public bool EventBlocked()
        {
            var evBlock = eventBlocked;
            eventBlocked = false;
            return evBlock;
        }
    }
}