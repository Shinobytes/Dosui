using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace Shinobytes.Console.Forms.Graphics
{
    public abstract class ConsoleAppBase : IDisposable
    {
        public static int UPDATE_INTERVAL = 16;

        private readonly object stateLock = new object();
        private readonly ConsoleGraphics graphics;
        //private readonly SafeFileHandle consoleHandle;
        private readonly IntPtr consoleHandle;

        private bool interrupt;
        private bool enabled;
        private bool isDisposed;
        private ColorPalette lastPalette;

        private const int STD_OUTPUT_HANDLE = -11;
        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint CONSOLE_TEXTMODE_BUFFER = 0x00000001;
        private readonly IntPtr NULL = IntPtr.Zero;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess,
            [MarshalAs(UnmanagedType.U4)] uint fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] int flags,
            IntPtr template);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutput(
            // SafeFileHandle hConsoleOutput,
            IntPtr hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteConsoleOutputW(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfoEx(
            IntPtr hConsoleOutput,
            ref ConsoleScreenBufferInfoEx csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleScreenBufferInfoEx(
            IntPtr hConsoleOutput,
            ref ConsoleScreenBufferInfoEx csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleOutputCP(uint wCodePageID);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("api-ms-win-core-winrt-string-l1-1-0.dll")]
        private static extern int WindowsCreateString([MarshalAs(UnmanagedType.LPWStr)]string sourceString, int stringLength, out IntPtr hstring);

        [DllImport("api-ms-win-core-winrt-string-l1-1-0.dll")]
        private static extern int WindowsDeleteString(IntPtr hstring);

        [DllImport("api-ms-win-core-winrt-l1-1-0.dll")]
        private static extern int RoGetActivationFactory(IntPtr className, ref Guid guid, out IntPtr instance);

        private CharInfo[] previousScreenBuffer;

        protected ConsoleAppBase(int width, int height)
            : this(new ConsoleGraphics(width, height))
        {
        }

        private ConsoleAppBase(ConsoleGraphics graphics, bool startEmmediately = false)
        {
            System.Console.CursorVisible = false;
            System.Console.SetWindowSize(graphics.Width, graphics.Height);
            System.Console.SetBufferSize(graphics.Width, graphics.Height);
            System.Console.OutputEncoding = System.Text.Encoding.Unicode;

            this.graphics = graphics;

            this.consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);


            if (HasAnniversaryUpdate())
            {
                if (GetConsoleMode(consoleHandle, out var mode))
                {
                    if (SetConsoleMode(consoleHandle, mode | 0x4))
                    {
                        this.ExtendedColorMode = true;
                    }
                }
            }

            new Thread(RenderLoop).Start();
            if (startEmmediately) Run();
        }

        public bool ExtendedColorMode { get; }

        public ConsoleGraphics Graphics => graphics;

        private void RenderLoop()
        {
            var start = DateTime.Now;
            var lastFrame = DateTime.Now;
            while (true)
            {
                var frameStart = DateTime.Now;
                var elapsed = frameStart - lastFrame;
                var totalElapsed = DateTime.Now - start;
                var time = new AppTime(elapsed.TotalSeconds, totalElapsed.TotalSeconds);
                lock (stateLock)
                {
                    if (interrupt) return;
                    if (!enabled) continue;
                }


                Update(time);
                Draw(time);

                DrawToConsole();

                lastFrame = frameStart;

                // this should keep the CPU usage down to a minimum
                // since we dont need super fast rendering, this is fine.
                Thread.Sleep(UPDATE_INTERVAL);
            }
        }

        protected abstract void Draw(AppTime appTime);

        protected abstract void Update(AppTime appTime);

        public abstract void OnKeyDown(KeyInfo keyInfo);

        private void DrawToConsole()
        {
            UpdateConsoleColorPalette();

            var screenBuffer = (CharInfo[])graphics;

            var buffer = GetBufferDiff(screenBuffer, out var rect);
            if (buffer != null)
            {
                WriteConsoleOutput(this.consoleHandle, buffer,
                    new Coord { X = rect.Right, Y = rect.Bottom },
                    new Coord { X = rect.Left, Y = rect.Top },
                    ref rect);
            }
        }

        private CharInfo[] GetBufferDiff(CharInfo[] screenBuffer, out SmallRect smallRect)
        {

            try
            {
                var rect = new SmallRect
                {
                    Left = 0,
                    Top = 0,
                    Right = (short)graphics.Width,
                    Bottom = (short)graphics.Height
                };

                smallRect = rect;

                if (previousScreenBuffer == null || previousScreenBuffer.Length != screenBuffer.Length)
                {
                    return screenBuffer;
                }

                for (var y = 0; y < graphics.Height; y++)
                {
                    for (var x = 0; x < graphics.Width; x++)
                    {
                        var index = y * graphics.Width + x;
                        var c1 = screenBuffer[index];
                        var c2 = previousScreenBuffer[index];
                        if (c1.Attributes != c2.Attributes || c1.UnicodeChar != c2.UnicodeChar)
                        {
                            return screenBuffer;
                        }
                    }
                }

                return null;
            }
            finally
            {
                previousScreenBuffer = new CharInfo[screenBuffer.Length];
                screenBuffer.CopyTo(previousScreenBuffer, 0);
            }
        }


        //private CharInfo[] GetBufferDiff(CharInfo[] screenBuffer, out SmallRect smallRect)
        //{
        //    var startX = -1;
        //    var startY = -1;
        //    var endX = 0;
        //    var endY = 0;

        //    try
        //    {
        //        if (previousScreenBuffer == null || previousScreenBuffer.Length != screenBuffer.Length)
        //        {
        //            var rect = new SmallRect
        //            {
        //                Left = 0,
        //                Top = 0,
        //                Right = (short)graphics.Width,
        //                Bottom = (short)graphics.Height
        //            };
        //            smallRect = rect;
        //            return screenBuffer;
        //        }

        //        for (var y = 0; y < graphics.Height; y++)
        //        {
        //            for (var x = 0; x < graphics.Width; x++)
        //            {
        //                var index = y * graphics.Width + x;
        //                var c1 = screenBuffer[index];
        //                var c2 = previousScreenBuffer[index];
        //                if (c1.Attributes != c2.Attributes || c1.UnicodeChar != c2.UnicodeChar)
        //                {
        //                    if (startX == -1 || x < startX) startX = x;
        //                    if (startY == -1 || y < startY) startY = y;
        //                    if (x > endX) endX = x;
        //                    if (y > endY) endY = y;
        //                }
        //            }
        //        }

        //        if (startX < 0 || startY < 0)
        //        {
        //            smallRect = new SmallRect();
        //            return null;
        //        }

        //        smallRect = new SmallRect
        //        {
        //            Left = 0,
        //            Top = 0,
        //            Right = (short)graphics.Width,
        //            Bottom = (short)graphics.Height
        //        };

        //        return screenBuffer;
        //    }
        //    finally
        //    {
        //        previousScreenBuffer = new CharInfo[screenBuffer.Length];
        //        screenBuffer.CopyTo(previousScreenBuffer, 0);
        //    }
        //}

        private static ColorRef Ref(Color c)
        {
            return new ColorRef(c);
        }

        private void UpdateConsoleColorPalette()
        {
            var palette = Graphics.Palette;
            if (palette == null || lastPalette == palette) return;
            var info = new ConsoleScreenBufferInfoEx();
            info.cbSize = Marshal.SizeOf(info);
            if (!GetConsoleScreenBufferInfoEx(consoleHandle, ref info))
            {
                throw new Exception();
            }
            for (var i = 0; i < palette.Entries.Length; i++)
            {
                switch (i)
                {
                    case 0: info.black = Ref(palette.Entries[i]); break;
                    case 1: info.darkBlue = Ref(palette.Entries[i]); break;
                    case 2: info.darkGreen = Ref(palette.Entries[i]); break;
                    case 3: info.darkCyan = Ref(palette.Entries[i]); break;
                    case 4: info.darkRed = Ref(palette.Entries[i]); break;
                    case 5: info.darkMagenta = Ref(palette.Entries[i]); break;
                    case 6: info.darkYellow = Ref(palette.Entries[i]); break;
                    case 7: info.gray = Ref(palette.Entries[i]); break;
                    case 8: info.darkGray = Ref(palette.Entries[i]); break;
                    case 9: info.blue = Ref(palette.Entries[i]); break;
                    case 10: info.green = Ref(palette.Entries[i]); break;
                    case 11: info.cyan = Ref(palette.Entries[i]); break;
                    case 12: info.red = Ref(palette.Entries[i]); break;
                    case 13: info.magenta = Ref(palette.Entries[i]); break;
                    case 14: info.yellow = Ref(palette.Entries[i]); break;
                    case 15: info.white = Ref(palette.Entries[i]); break;
                }
            }

            info.srWindow.Bottom++;
            info.srWindow.Right++;

            if (!SetConsoleScreenBufferInfoEx(consoleHandle, ref info))
            {
                throw new Exception();
            }
            lastPalette = Graphics.Palette;
        }

        public void Run()
        {
            lock (stateLock)
            {
                enabled = true;
            }
        }

        public void Stop()
        {
            lock (stateLock)
            {
                enabled = false;
                interrupt = true;
            }
        }

        public void Dispose()
        {
            if (isDisposed) return;
            Stop();
            isDisposed = true;
        }

        private static bool HasAnniversaryUpdate()
        {
            const string ClassName = "Windows.ApplicationModel.AppExtensions.AppExtensionCatalog";
            var stringHandle = IntPtr.Zero;
            try
            {
                if (WindowsCreateString(ClassName, ClassName.Length, out stringHandle) != 0)
                {
                    return false;
                }

                var appExtensionCatalogStaticsId =
                    new Guid(1010198154, 24344, 20235, 156, 229, 202, 182, 29, 25, 111, 17);

                if (RoGetActivationFactory(
                        stringHandle,
                        ref appExtensionCatalogStaticsId,
                        out var appExtensionCatalogStatics) != 0)
                {
                    return false;
                }

                if (appExtensionCatalogStatics != IntPtr.Zero)
                {
                    Marshal.Release(appExtensionCatalogStatics);
                    return true;
                }
            }
            finally
            {
                if (IntPtr.Zero != stringHandle)
                {
                    WindowsDeleteString(stringHandle);
                }
            }
            return false;
        }
    }
}