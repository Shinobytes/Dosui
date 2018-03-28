using System;
using System.Drawing;
using System.Linq;

namespace Shinobytes.Console.Forms
{
    public class MessageBox
    {
        public static void Show(
            string message,
            string caption,
            MessageBoxButtons buttons,
            Action<bool?> callback = null)
        {
            var msgBoxWindow = new MessageBoxWindow();
            var startingRows = message.Count(x => x == '\n');
            var msgLen = message.Split('\n').Max(x => x.Length);

            msgBoxWindow.Size = new Size(Math.Max(20, Math.Max(caption.Length, msgLen + 2) + 4), 7 + startingRows);
            msgBoxWindow.Position = new Point(Application.Current.Graphics.Width / 2 - msgBoxWindow.Size.Width / 2, 10);

            msgBoxWindow.Text = caption;
            msgBoxWindow.Message = message;

            msgBoxWindow.SetupButtons(buttons);
            msgBoxWindow.Show(callback);
        }
    }
}