using System;
using System.Drawing;
using System.Linq;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    internal class MessageBoxWindow : Window
    {
        private readonly TextBlock label;

        internal MessageBoxWindow()
        {
            IsDialog = true;
            BackgroundColor = ConsoleColor.Gray;

            label = new TextBlock();
            label.ForegroundColor = ConsoleColor.Black;
            label.BackgroundColor = this.BackgroundColor;
            label.Position = new Point(1, 1);
            Controls.Add(label);
        }

        public string Message
        {
            get => label.Text;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    var rows = value.Split('\n').SelectMany(x => SplitIfTooLong(x, this.Size.Width - 4));
                    label.Text = string.Join("\n", rows);
                    return;
                }

                label.Text = value;
            }
        }

        private string[] SplitIfTooLong(string input, int maxSize)
        {
            if (input.Length < maxSize || maxSize <= 0)
            {
                return new[] { input };
            }
            // with word wrap, so go from expected char to be cut at and go until we find a space
            // if no space is found, cut at original pos

            var cutIndex = maxSize - 1;
            for (var i = cutIndex; i > 0; i--)
            {
                if (input[i] == ' ' || input[i] == ',' || input[i] == ';' || input[i] == '-')
                {
                    cutIndex = i;
                    if (input[i] == ' ') cutIndex++; // don't include the space on the newline, thats just gross
                    break;
                }
            }

            return new[]
            {
                input.Remove(cutIndex),
                input.Substring(cutIndex)
            };

        }

        public void SetupButtons(MessageBoxButtons buttons)
        {
            var doubleButtons = buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.OKCancel;
            var yesText = buttons == MessageBoxButtons.YesNo ? "Yes" : "OK";
            var noText = buttons == MessageBoxButtons.YesNo ? "No" : "Cancel";
            var xOffset = doubleButtons ? 12 : 6;
            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.OKCancel || buttons == MessageBoxButtons.OK)
            {
                var okBtn = new Button()
                {
                    Text = yesText,
                    BackgroundColor = ConsoleColor.DarkGreen,
                    ForegroundColor = ConsoleColor.White,
                    Position = new Point(
                        (this.Size.Width / 2) - xOffset,
                        this.Size.Height - 4),
                    Size = new Size(8, 1)
                };
                okBtn.Invoke += (sender, args) =>
                {
                    this.DialogResult = true;
                };
                Controls.Add(okBtn);
            }

            if (buttons == MessageBoxButtons.OKCancel || buttons == MessageBoxButtons.YesNo)
            {
                var cancelBtn = new Button()
                {
                    Text = noText,
                    BackgroundColor = ConsoleColor.DarkGray,
                    ForegroundColor = ConsoleColor.White,
                    Position = new Point(
                        this.Size.Width / 2,
                        this.Size.Height - 4),
                    Size = new Size(8, 1)
                };
                cancelBtn.Invoke += (sender, args) =>
                {
                    this.DialogResult = false;
                };
                Controls.Add(cancelBtn);
            }
        }

        public override void Update(AppTime appTime)
        {
            var newPosX = Application.Current.Graphics.Width / 2 - this.Size.Width / 2;
            if (newPosX >= 0)
            {
                this.Position = new Point(newPosX, 10);
            }
            base.Update(appTime);
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                this.DialogResult = false;
                return false;
            }
            return base.OnKeyDown(key);
        }
    }
}