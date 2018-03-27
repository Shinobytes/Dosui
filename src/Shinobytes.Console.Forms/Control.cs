using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public abstract class Control
    {
        private bool eventBlocked = false;
        private ConsoleColor backgroundColor = ConsoleColor.Black;

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

        public ConsoleColor BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                TransparentBackground = false;
            }
        }

        public string Text { get; set; }

        public virtual string RenderText => Text;

        internal IReadOnlyList<DrawStringOperation> TextRenderOperations => ParseDrawOperations(this.RenderText);

        public int TabIndex { get; set; }
        public bool Visible { get; set; } = true;
        public bool IsEnabled { get; set; } = true;
        public bool HasFocus { get; internal set; } = false;
        public bool TransparentBackground { get; set; } = true;

        public ConsoleColor ForegroundColor { get; set; } = ConsoleColor.White;
        public ConsoleColor DisabledForegroundColor { get; set; } = ConsoleColor.DarkGray;

        public Size Size { get; set; }
        public Point Position { get; set; }

        public Control Parent { get; internal set; }
        public virtual bool ShortcutListener => !string.IsNullOrEmpty(this.Text) && this.Text.Contains("&");
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

        internal IReadOnlyList<DrawStringOperation> ParseDrawOperations(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new List<DrawStringOperation>();
            }

            return text.Split('&').Select((x, y) =>
                y == 0
                    ? new[] { new DrawStringOperation(x, this.ForegroundColor, this.BackgroundColor) }
                    : new[] {
                        new DrawStringOperation(x[0].ToString(), Application.ThemeColor, this.BackgroundColor),
                        new DrawStringOperation(x.Substring(1), this.ForegroundColor, this.BackgroundColor) })
                        .SelectMany(x => x)
                        .ToList();

            //// just a super simple parser/tokenizer to handle the & op, faster though
            //var list = new List<DrawStringOperation>();
            //var index = 0;
            //do
            //{
            //    var str = "";
            //    var token = text[index];
            //    switch (token)
            //    {
            //        case '&':
            //            list.Add(new DrawStringOperation(text[++index].ToString(), ThemeColor, this.BackgroundColor));
            //            break;
            //        default:
            //            str += text[index];
            //            if (index + 1 < text.Length)
            //            {
            //                var next = text[index + 1];
            //                while (index + 1 < text.Length)
            //                {
            //                    if (text[index + 1] == '&') break;
            //                    next = text[++index];
            //                    str += next;
            //                }
            //            }
            //            list.Add(new DrawStringOperation(str, this.ForegroundColor, this.BackgroundColor));
            //            break;
            //    }
            //    ++index;
            //} while (index < text.Length);
            //return list;
        }

        protected bool HandleNavigationKeys(KeyInfo key)
        {
            if (!IsNavigationKey(key.Key))
                return true; // continue as if nothing happened
            if (Parent != null)
            {
                return Parent.Navigation(this, key.Key);
            }
            return Navigation(this, key.Key);
        }

        public virtual bool Navigation(Control sender, ConsoleKey key)
        {
            return true;
        }

        internal bool IsNavigationKey(ConsoleKey key)
        {
            return key == ConsoleKey.Enter || key == ConsoleKey.Escape || key == ConsoleKey.LeftArrow ||
                   key == ConsoleKey.UpArrow || key == ConsoleKey.RightArrow || key == ConsoleKey.DownArrow;
        }
    }
}