using System;
using System.Drawing;
using Shinobytes.Console.Forms;

namespace SampleConsoleApp
{
    public class TestConfirmationWindow : Window
    {
        public TestConfirmationWindow()
        {
            BackgroundColor = ConsoleColor.Gray;
            Text = " So, do you? ";
            Size = new Size(40, 8);
            Position = new Point(40, 5);

            Controls.Add(new TextBlock("Do you like Bananas?", ConsoleColor.Red, new Point(8, 1)));

            var yesBtn = new Button
            {
                Text = "Yes",
                Position = new Point(7, 4),
                Size = new Size(7, 1),
                ForegroundColor = ConsoleColor.White,
                BackgroundColor = ConsoleColor.DarkCyan,
                DropShadow = true
            };
            yesBtn.Invoke += (sender, args) =>
            {
                this.DialogResult = true;
            };
            Controls.Add(yesBtn);

            var noBtn = new Button
            {
                Text = "No",
                Position = new Point(22, 4),
                Size = new Size(6, 1),
                BackgroundColor = ConsoleColor.White,
                DropShadow = true
            };
            noBtn.Invoke += (sender, args) =>
            {
                this.DialogResult = false;
            };
            Controls.Add(noBtn);
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            if (key.Key == ConsoleKey.F2 || key.Key == ConsoleKey.Escape)
            {
                this.Hide();
            }
            return base.OnKeyDown(key);
        }
    }
}