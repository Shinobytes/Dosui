using System;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public enum ProgressValuePosition
    {
        Above,
        Inside,
        Below
    }

    public class ProgressBar : Control
    {
        public float MaxValue { get; set; } = 100f;
        public float MinValue { get; set; } = 0f;
        public float Value { get; set; }

        public BorderThickness BorderThickness { get; set; } = new BorderThickness(1, 1, 1, 1);
        public ConsoleColor BorderColor { get; set; } = ConsoleColor.White;

        public ConsoleColor ProgressBackColor { get; set; } = ConsoleColor.DarkGray;
        public ConsoleColor ProgressFrontColor { get; set; } = ConsoleColor.White;
        public bool Indeterminate { get; set; }
        public bool ProgressValueVisible { get; set; } = true;
        public ProgressValuePosition ProgressValuePosition { get; set; } = ProgressValuePosition.Inside;
        public float AnimationSpeed { get; set; } = 5f;

        private float animationPositionX = 0;


        public ProgressBar()
        {
            CanFocus = false;
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            if (this.BorderThickness.Size > 0)
                graphics.DrawBorder(
                    this.BorderThickness,
                    this.Position.X,
                    this.Position.Y,
                    this.Size.Width + 1,
                    this.Size.Height + 1,
                    this.BorderColor,
                    this.BackgroundColor);


            // following can be optimized by first drawing parts that is filled
            // and then draw remaining as background

            if (Indeterminate)
            {
                var width = this.Size.Width / 4;
                var renderWidth = (int)Math.Max(1, Math.Min(width, (this.Size.Width + 1) - animationPositionX));

                var offsetX = (int)animationPositionX;

                for (var row = 0; row < this.Size.Height; row++)
                {
                    graphics.DrawLineChar(
                        AsciiCodes.ProgressBar_Background,
                        this.Position.X + 1,
                        this.Position.Y + 1 + row,
                        this.Position.X + 1 + this.Size.Width,
                        this.Position.Y + 1 + row,
                        this.ProgressBackColor,
                        this.ProgressBackColor);

                    graphics.DrawLineChar(
                        AsciiCodes.ProgressBar_Foreground,
                        this.Position.X + 1 + offsetX,
                        this.Position.Y + 1 + row,
                        this.Position.X + 1 + offsetX + renderWidth,
                        this.Position.Y + 1 + row,
                        this.ProgressFrontColor,
                        this.ProgressFrontColor);
                }
            }
            else
            {

                for (var row = 0; row < this.Size.Height; row++)
                {
                    graphics.DrawLineChar(
                        AsciiCodes.ProgressBar_Background,
                        this.Position.X + 1,
                        this.Position.Y + 1 + row,
                        this.Position.X + 1 + this.Size.Width,
                        this.Position.Y + 1 + row,
                        this.ProgressBackColor,
                        this.ProgressBackColor);


                    var proc = (Value / (this.MaxValue - this.MinValue));
                    var width = (int)(this.Size.Width * proc);
                    graphics.DrawLineChar(
                        AsciiCodes.ProgressBar_Foreground,
                        this.Position.X + 1,
                        this.Position.Y + 1 + row,
                        this.Position.X + 1 + width,
                        this.Position.Y + 1 + row,
                        this.ProgressFrontColor,
                        this.ProgressFrontColor);
                }

                if (ProgressValueVisible)
                {
                    var text = this.Value + "%";
                    var textX = this.Position.X + (Size.Width / 2 - text.Length / 2) + 2;
                    var textY = this.Position.Y + (int)(Size.Height / 2f - 0.5);

                    if (this.ProgressValuePosition == ProgressValuePosition.Inside)
                    {
                        textY += 1;
                        for (var i = 0; i < text.Length; i++)
                        {
                            var sampleFg = graphics.GetForeground(textX + i, textY);
                            var fg = sampleFg == this.ProgressFrontColor ? this.BackgroundColor : this.ForegroundColor;
                            var bg = sampleFg == this.ProgressFrontColor ? this.ProgressFrontColor : this.ProgressBackColor;
                            graphics.SetPixelChar(text[i], textX + i, textY, fg, bg);
                        }
                        return;
                    }
                    else if (this.ProgressValuePosition == ProgressValuePosition.Above)
                    {
                        textY -= 1;
                    }
                    else
                    {
                        textY += this.Size.Height + 2;
                    }

                    graphics.DrawString(text, textX, textY, ForegroundColor, BackgroundColor);
                }
            }
        }

        public override void Update(AppTime appTime)
        {
            if (!Indeterminate)
            {
                return;
            }

            animationPositionX += (float)(appTime.Elapsed * AnimationSpeed);
            if (animationPositionX >= this.Size.Width)
            {
                animationPositionX = 0;
            }
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            return false;
        }
    }
}
