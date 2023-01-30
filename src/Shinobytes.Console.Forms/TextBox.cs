using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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

        public char PasswordChar { get; set; } = '*';
        public bool Password { get; set; }

        public TextBox()
        {
            ForegroundColor = ConsoleColor.Black;
            Text = string.Empty;
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
                graphics.DrawString(this.Password ? new string(PasswordChar, this.Text.Length) : this.Text, posX, posY, this.ForegroundColor, this.BackgroundColor);
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

        public override void Focus()
        {
            base.Focus();
            this.cursorAnimationState = true;
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            var topBorder = this.BorderThickness.Top > 0 ? 1 : 0;
            var rightBorder = this.BorderThickness.Right > 0 ? 1 : 0;
            var width = AreaEdit ? this.Size.Width : (this.Text?.Length + 1) ?? 0;
            var height = AreaEdit ? this.Size.Height : 1;

            if (key.KeyChar == '{')
            {
                // ctrl+v ?
                var text = WindowsClipboard.GetText();
                if (!string.IsNullOrEmpty(text))
                {
                    PasteText(text);
                }
                return false;
            }

            if (!AreaEdit && key.Key == ConsoleKey.Enter && this.Parent is ContainerControl container)
            {
                return !container.TryFocusNext();
            }
            else if (this.IsArrowKey(key.Key))
            {
                if (key.Key == ConsoleKey.RightArrow)
                {
                    MoveCursorRight();
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    MoveCursorLeft();
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
                            return HandleNavigationKeys(key);
                        }
                    }

                    if (key.Key == ConsoleKey.DownArrow)
                    {
                        this.CursorY++;
                        if (this.CursorY > (height - topBorder))
                        {
                            this.CursorY = height - topBorder;
                            return HandleNavigationKeys(key);
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
                this.cursorAnimationState = true;
            }
            else if (key.Key == ConsoleKey.End)
            {
                this.CursorX = width - rightBorder;
                this.cursorAnimationState = true;
            }
            else if (key.Key == ConsoleKey.PageUp)
            {
                this.CursorY = 0; // for now
                this.cursorAnimationState = true;
            }
            else if (key.Key == ConsoleKey.PageDown)
            {
                this.CursorY = height - topBorder;
                this.cursorAnimationState = true;
            }
            else
            {
                EnsureTextSize();

                if (AreaEdit)
                {
                    AreaTextInsert(key.KeyChar + "");
                }
                else
                {
                    this.Text = this.Text.Insert(this.CursorX, key.KeyChar + "");
                    this.CursorX++;
                }
            }

            return false;
        }

        private void PasteText(string text)
        {
            EnsureTextSize();
            if (AreaEdit)
            {
                return; // not supported yet.
            }

            this.Text = this.Text.Insert(this.CursorX, text);
        }

        private void AreaTextInsert(string text)
        {
            // multi line text, fill a char grid [x,y] using width, height
            //this.Text = string.Join(Environment.NewLine)

            this.Text = this.Text.Insert(this.CursorX, text);
            this.CursorX++;
        }

        private void EnsureTextSize()
        {
            if (!this.AreaEdit)
            {
                if (Text == null)
                {
                    Text = string.Empty;
                }

                if (Text.Length < this.CursorX)
                {
                    Text = Text.PadRight(this.CursorX, ' ');
                }
                return;
            }

            // build multi line text
        }

        private void MoveCursorRight()
        {
            var topBorder = this.BorderThickness.Top > 0 ? 1 : 0;
            var rightBorder = this.BorderThickness.Right > 0 ? 1 : 0;
            var width = AreaEdit ? this.Size.Width : (this.Text?.Length + 1) ?? 0;
            var height = AreaEdit ? this.Size.Height : 1;

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

        private void MoveCursorLeft()
        {
            var rightBorder = this.BorderThickness.Right > 0 ? 1 : 0;
            var width = AreaEdit ? this.Size.Width : (this.Text?.Length + 1) ?? 0;

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
    }
}

public static class WindowsClipboard
{
    public static async Task SetTextAsync(string text, CancellationToken cancellation)
    {
        await TryOpenClipboardAsync(cancellation);

        InnerSet(text);
    }

    public static void SetText(string text)
    {
        TryOpenClipboard();

        InnerSet(text);
    }

    static void InnerSet(string text)
    {
        EmptyClipboard();
        IntPtr hGlobal = default;
        try
        {
            var bytes = (text.Length + 1) * 2;
            hGlobal = Marshal.AllocHGlobal(bytes);

            if (hGlobal == default)
            {
                ThrowWin32();
            }

            var target = GlobalLock(hGlobal);

            if (target == default)
            {
                ThrowWin32();
            }

            try
            {
                Marshal.Copy(text.ToCharArray(), 0, target, text.Length);
            }
            finally
            {
                GlobalUnlock(target);
            }

            if (SetClipboardData(cfUnicodeText, hGlobal) == default)
            {
                ThrowWin32();
            }

            hGlobal = default;
        }
        finally
        {
            if (hGlobal != default)
            {
                Marshal.FreeHGlobal(hGlobal);
            }

            CloseClipboard();
        }
    }

    static async Task TryOpenClipboardAsync(CancellationToken cancellation)
    {
        var num = 10;
        while (true)
        {
            if (OpenClipboard(default))
            {
                break;
            }

            if (--num == 0)
            {
                ThrowWin32();
            }

            await Task.Delay(100, cancellation);
        }
    }

    static void TryOpenClipboard()
    {
        var num = 10;
        while (true)
        {
            if (OpenClipboard(default))
            {
                break;
            }

            if (--num == 0)
            {
                ThrowWin32();
            }

            Thread.Sleep(100);
        }
    }

    public static async Task<string> GetTextAsync(CancellationToken cancellation)
    {
        if (!IsClipboardFormatAvailable(cfUnicodeText))
        {
            return null;
        }
        await TryOpenClipboardAsync(cancellation);

        return InnerGet();
    }

    public static string GetText()
    {
        if (!IsClipboardFormatAvailable(cfUnicodeText))
        {
            return null;
        }
        TryOpenClipboard();

        return InnerGet();
    }

    static string InnerGet()
    {
        IntPtr handle = default;

        IntPtr pointer = default;
        try
        {
            handle = GetClipboardData(cfUnicodeText);
            if (handle == default)
            {
                return null;
            }

            pointer = GlobalLock(handle);
            if (pointer == default)
            {
                return null;
            }

            var size = GlobalSize(handle);
            var buff = new byte[size];

            Marshal.Copy(pointer, buff, 0, size);

            return Encoding.Unicode.GetString(buff).TrimEnd('\0');
        }
        finally
        {
            if (pointer != default)
            {
                GlobalUnlock(handle);
            }

            CloseClipboard();
        }
    }

    const uint cfUnicodeText = 13;

    static void ThrowWin32()
    {
        throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    [DllImport("User32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool IsClipboardFormatAvailable(uint format);

    [DllImport("User32.dll", SetLastError = true)]
    static extern IntPtr GetClipboardData(uint uFormat);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr GlobalLock(IntPtr hMem);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GlobalUnlock(IntPtr hMem);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool CloseClipboard();

    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr SetClipboardData(uint uFormat, IntPtr data);

    [DllImport("user32.dll")]
    static extern bool EmptyClipboard();

    [DllImport("Kernel32.dll", SetLastError = true)]
    static extern int GlobalSize(IntPtr hMem);
}
