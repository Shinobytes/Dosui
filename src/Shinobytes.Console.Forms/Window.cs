using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class Window : ContainerControl
    {
        private bool closed;
        private bool? dialogResult;
        private TaskCompletionSource<bool?> completionSource;
        private readonly List<Action<bool?>> closeCallbacks = new List<Action<bool?>>();

        public event EventHandler<bool?> Closed;

        public bool IsMainWindow { get; internal set; }
        public ConsoleColor CaptionColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor BorderColor { get; set; } = ConsoleColor.DarkGray;

        internal bool IsDialog { get; set; }

        public Window()
        {
            WindowManager.Register(this);
            Parent = WindowManager.MainWindow;

            Visible = false;
            BackgroundColor = ConsoleColor.DarkBlue;
            ForegroundColor = ConsoleColor.Black;
        }


        public bool? DialogResult
        {
            get => dialogResult;
            set
            {
                // close dialog
                dialogResult = value;

                this.Close();
            }
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            if (!Visible || Size.Width * Size.Height <= 0) return;
            if (IsMainWindow)
            {
                DrawWindowFullSize(graphics, appTime);
            }
            else
            {
                DrawWindow(graphics, appTime);
            }
        }

        public override void Focus()
        {
            if (!this.ActiveControl)
            {
                var focusableControl = this.Controls.OrderBy(x => x.TabIndex).FirstOrDefault(x => x.CanFocus);
                if (focusableControl)
                {
                    focusableControl.Focus();
                }
            }

            base.Focus();
        }

        private void DrawWindow(IGraphics graphics, AppTime appTime)
        {
            // window background
            graphics.Clear(Position.X, Position.Y, Size.Width, Size.Height, BackgroundColor);

            // window background shadow drop
            //  - right shadow

            var borderPaddingX = 1;

            for (var i = 0; i < Size.Height; i++)
            {
                // drop shadow, right side
                var c0 = graphics.GetPixelChar(Position.X + Size.Width, Position.Y + i + 1);
                var c1 = graphics.GetPixelChar(Position.X + Size.Width + 1, Position.Y + i + 1);

                // 1. get char at x,y
                // 2. set pixel char, same char but with black bg and gray foreground to create a "shadow" ontop of text.
                graphics.SetPixelChar(c0, Position.X + Size.Width, Position.Y + i + 1, ConsoleColor.DarkGray, ConsoleColor.Black);
                graphics.SetPixelChar(c1, Position.X + Size.Width + 1, Position.Y + i + 1, ConsoleColor.DarkGray, ConsoleColor.Black);

                // borders, both left and right
                if (i == 0 || i == Size.Height) continue;
                graphics.SetPixelChar(AsciiCodes.BorderSingle_Vertical, Position.X + borderPaddingX, Position.Y + i, this.BorderColor, this.BackgroundColor);
                graphics.SetPixelChar(AsciiCodes.BorderSingle_Vertical, Position.X + Size.Width - (1 + borderPaddingX), Position.Y + i, this.BorderColor, this.BackgroundColor);
            }


            // window caption background and border bottom
            for (var i = 0; i < Size.Width; i++)
            {
                // bottom shadow
                var c0 = graphics.GetPixelChar(Position.X + 1 + i, Position.Y + Size.Height);
                graphics.SetPixelChar(c0, Position.X + 1 + i, Position.Y + Size.Height, ConsoleColor.DarkGray, ConsoleColor.Black);

                // border bottom and top
                if (i <= borderPaddingX || i >= Size.Width - (1 + borderPaddingX)) continue;
                graphics.SetPixelChar(AsciiCodes.BorderSingle_Horizontal, Position.X + i, Position.Y, this.BorderColor, this.BackgroundColor);
                graphics.SetPixelChar(AsciiCodes.BorderSingle_Horizontal, Position.X + i, Position.Y + this.Size.Height - 1, this.BorderColor, this.BackgroundColor);
            }

            // draw top border corners
            graphics.SetPixelChar(AsciiCodes.BorderSingle_TopLeft, Position.X + borderPaddingX, Position.Y, this.BorderColor, this.BackgroundColor);
            graphics.SetPixelChar(AsciiCodes.BorderSingle_TopRight, Position.X - borderPaddingX + Size.Width - 1, Position.Y, this.BorderColor, this.BackgroundColor);

            // draw bottom border corners
            graphics.SetPixelChar(AsciiCodes.BorderSingle_BottomLeft, Position.X + borderPaddingX, Position.Y + this.Size.Height - 1, this.BorderColor, this.BackgroundColor);
            graphics.SetPixelChar(AsciiCodes.BorderSingle_BottomRight, Position.X - borderPaddingX + Size.Width - 1, Position.Y + this.Size.Height - 1, this.BorderColor, this.BackgroundColor);

            // window caption text
            graphics.DrawString(Text, (int)(Position.X + (Size.Width / 2f - Text.Length / 2f)), Position.Y, ForegroundColor, CaptionColor);

            using (var gfx = graphics.CreateViewport(this.Position.X + 2, this.Position.Y + 1))
            {
                // finally draw all child components last
                base.Draw(gfx, appTime);
            }
        }

        private void DrawWindowFullSize(IGraphics graphics, AppTime appTime)
        {
            // window background
            graphics.Clear(BackgroundColor);

            // draw children as is, since our container is taking up the whole application
            base.Draw(graphics, appTime);
        }

        public void Show()
        {
            if (this.Visible) return;

            WindowManager.Register(this);
            this.Focus();
            Visible = true;
            var firstFocusableControl = this.Controls.OrderBy(x => x.TabIndex).FirstOrDefault(x => x.CanFocus);
            if (firstFocusableControl) firstFocusableControl.Focus();
        }

        public void Show(Action<bool?> callback)
        {
            if (this.Visible) return;
            this.Show();
            if (callback != null)
                this.closeCallbacks.Add(callback);
        }

        public void Close()
        {
            WindowManager.Unregister(this);

            Visible = false;
            IsEnabled = false;
            closed = true;
            Blur();

            Closed?.Invoke(this, this.DialogResult);

            if (completionSource != null && !completionSource.Task.IsCompleted)
            {
                completionSource.TrySetResult(dialogResult);
            }

            foreach (var callback in closeCallbacks)
            {
                callback?.Invoke(this.dialogResult);
            }

            closeCallbacks.Clear();
        }

        //public Task<bool?> ShowDialogAsync()
        //{
        //    this.Show();
        //    completionSource = new TaskCompletionSource<bool?>();
        //    return completionSource.Task;
        //}

        public void Hide()
        {
            Visible = false;
            this.Blur();
        }
    }
}
