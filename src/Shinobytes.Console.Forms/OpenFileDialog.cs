using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace Shinobytes.Console.Forms
{
    public class OpenFileDialog : Window
    {
        public string SelectedFile { get; private set; }
        public string InitialDirectory { get; set; }

        public OpenFileDialog()
        {
            Text = " Open file ";
            Size = new Size(40, 15);
            BackgroundColor = ConsoleColor.Gray;
            Position = new Point(Application.Current.Graphics.Width / 2 - Size.Width / 2, 6);

            Controls.Add(new TextBlock("-- Dialog not implemented --", ConsoleColor.Red, new Point(3, 1)));



            var tbx = new TextBox
            {
                Text = "Test",
                Placeholder = "Enter a name",
                Size = new Size(28, 1),
                Position = new Point(3, 3)
            };
            Controls.Add(tbx);





            var openBtn = new Button
            {
                Text = "Open",
                Size = new Size(10, 1),
                BackgroundColor = ConsoleColor.DarkGreen,
                ForegroundColor = ConsoleColor.White,
                Position = new Point(this.Size.Width - 28, this.Size.Height - 4)
            };
            openBtn.Invoke += (sender, args) =>
            {
                this.DialogResult = true;
            };
            this.Controls.Add(openBtn);

            var cancelBtn = new Button
            {
                Text = "Cancel",
                Size = new Size(10, 1),
                BackgroundColor = ConsoleColor.DarkGray,
                ForegroundColor = ConsoleColor.White,
                Position = new Point(this.Size.Width - 16, this.Size.Height - 4)
            };
            cancelBtn.Invoke += (sender, args) =>
            {
                this.DialogResult = false;
            };
            this.Controls.Add(cancelBtn);
        }
    }

    //public class SaveFileDialog : Window
    //{
    //}
}