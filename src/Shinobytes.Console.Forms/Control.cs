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

        public virtual void Focus()
        {
            InputManager.Focus(this);
        }

        public virtual void Blur()
        {
            InputManager.Blur(this);
        }

        private bool eventBlocked = false;
        private ConsoleColor backgroundColor = ConsoleColor.Black;

        public int TabIndex { get; set; }

        public bool Visible { get; set; } = true;
        public bool IsEnabled { get; set; } = true;

        public bool HasFocus { get; internal set; } = false;

        public bool TransparentBackground { get; set; } = true;

        public ConsoleColor BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                TransparentBackground = false;
            }
        }

        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;
        public ConsoleColor DisabledForegroundColor { get; set; } = ConsoleColor.DarkGray;

        public Size Size { get; set; }
        public Point Position { get; set; }

        public string Text { get; set; }
        public Control Parent { get; internal set; }
        public bool ShortcutListener { get; set; } = false;
        public bool CanFocus { get; set; } = true;


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