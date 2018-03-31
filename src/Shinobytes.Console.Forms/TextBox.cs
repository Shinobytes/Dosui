using System;
using Shinobytes.Console.Forms.Extensions;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class TextBox : Control
    {
        public BorderThickness BorderThickness { get; set; } = BorderThickness.One;
        public ConsoleColor CursorColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor BorderColor { get; set; } = ConsoleColor.DarkGray;
        public ConsoleColor PlaceholderColor { get; set; } = ConsoleColor.DarkGray;
        public bool CursorAnimation { get; set; } = true;
        public string Placeholder { get; set; }

        public int CursorX { get; set; }
        public int CursorY { get; set; }

        public bool AreaEdit { get; set; }

        private double cursorVisibleTimer = 1;
        private double cursorHiddenTimer = 0.5;
        private double cursorAnimationTimer = 1;
        private bool cursorAnimationState = true;



        public TextBox()
        {
            ForegroundColor = ConsoleColor.Black;
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            graphics.DrawRect(this.Position.X, this.Position.Y, this.Size.Width, this.Size.Height, this.BackgroundColor);
            var hasBorder = this.BorderThickness.Size > 0;
            var offsetXY = this.BorderThickness.Top > 0 ? 1 : 0;
            if (hasBorder)
            {
                graphics.DrawBorder(
                    this.BorderThickness,
                    this.Position.X,
                    this.Position.Y,
                    this.Size.Width + 1,
                    this.Size.Height + 1,
                    this.BorderColor,
                    this.BorderColor,
                    this.HasFocus ? Application.ThemeColor : this.BorderColor,
                    this.BorderColor,
                    this.BackgroundColor);
            }

            var posX = this.Position.X + offsetXY;
            var posY = this.Position.Y + offsetXY;
            if (!string.IsNullOrEmpty(Placeholder) && string.IsNullOrEmpty(this.Text))
            {
                graphics.DrawLine(posX, posY, posX + Size.Width, posY, this.BackgroundColor);
                graphics.DrawString(this.Placeholder, posX, posY, this.PlaceholderColor, this.BackgroundColor);
                //graphics.DrawLine(posX + (Size.Width - Placeholder.Length), posY, posX + Size.Width, posY, this.BackgroundColor);
            }
            else
            {
                graphics.DrawLine(posX, posY, posX + Size.Width, posY, this.BackgroundColor);
                graphics.DrawString(this.Text, posX, posY, this.ForegroundColor, this.BackgroundColor);
                //graphics.DrawLine(posX + (Size.Width - Text.Length), posY, posX + Size.Width, posY, this.BackgroundColor);
            }

            if (HasFocus && cursorAnimationState)
            {
                var cursorX = posX + CursorX;
                var cursorY = posY + CursorY;
                var sampledChar = graphics.GetPixelChar(cursorX, cursorY);
                graphics.SetPixel(sampledChar, cursorX, cursorY, this.BackgroundColor, this.CursorColor);
            }
        }

        public override void Update(AppTime appTime)
        {
            if (CursorAnimation)
            {
                cursorAnimationTimer -= appTime.Elapsed;
                if (cursorAnimationTimer <= 0)
                {
                    cursorAnimationState = !cursorAnimationState;
                    cursorAnimationTimer = cursorAnimationState ? cursorVisibleTimer : cursorHiddenTimer;
                }
            }
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            var topBorder = this.BorderThickness.Top > 0 ? 1 : 0;
            var rightBorder = this.BorderThickness.Right > 0 ? 1 : 0;
            var width = AreaEdit ? this.Size.Width : (this.Text?.Length + 1) ?? 0;
            var height = AreaEdit ? this.Size.Height : 1;
            if (this.IsArrowKey(key.Key))
            {
                if (key.Key == ConsoleKey.RightArrow)
                {
                    this.CursorX++;
                    if (this.CursorX >= width)
                    {
                        this.CursorX = width - rightBorder;
                        if (this.CursorY < (height - topBorder))
                        {
                            this.CursorX = 0;
                            this.CursorY++;
                        }
                    }
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    this.CursorX--;
                    if (this.CursorX < 0)
                    {
                        this.CursorX = 0;
                        if (this.CursorY > 0)
                        {
                            this.CursorX = width - rightBorder;
                            this.CursorY--;
                        }
                    }
                }
                else
                {
                    if (!AreaEdit)
                    {
                        return HandleNavigationKeys(key);// up and down should allow navigation if we are not an area edit                        
                    }

                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        this.CursorY--;
                        if (this.CursorY < 0)
                        {
                            this.CursorY = 0;
                        }
                    }

                    if (key.Key == ConsoleKey.DownArrow)
                    {
                        this.CursorY++;
                        if (this.CursorY >= (height - topBorder))
                        {
                            this.CursorY = height - topBorder;
                        }
                    }
                }

                // make sure the cursor is always visible when moving it
                this.cursorAnimationState = true;
                return false;
            }

            if (key.Key == ConsoleKey.Backspace)
            {
                if (this.Text.Length > 0)
                {
                    if (AreaEdit)
                    {
                        // ...
                    }
                    else
                    {
                        if (this.CursorX > 0)
                        {
                            this.CursorX--;
                            this.Text = this.Text.Remove(this.CursorX, 1);
                        }
                    }
                }
            }
            else if (key.Key == ConsoleKey.Delete)
            {
                if (this.CursorX <= width && !string.IsNullOrWhiteSpace(this.Text) && this.CursorX < this.Text.Length)
                {
                    this.Text = this.Text.Remove(this.CursorX, 1);
                }
            }
            else if (key.Key == ConsoleKey.Home)
            {
                this.CursorX = 0;
            }
            else if (key.Key == ConsoleKey.End)
            {
                this.CursorX = width - rightBorder;
            }
            else if (key.Key == ConsoleKey.PageUp)
            {
                this.CursorY = 0; // for now
            }
            else if (key.Key == ConsoleKey.PageDown)
            {
                // not implemented
            }
            else
            {
                this.Text = this.Text.Insert(this.CursorX, key.KeyChar + "");
                this.CursorX++;
            }

            return false;
        }
    }
}