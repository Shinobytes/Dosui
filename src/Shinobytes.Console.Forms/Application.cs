using System;
using System.Drawing;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class Application : ConsoleAppBase
    {
        private readonly Window mainWindow;

        public static ConsoleColor ThemeColor { get; set; } = ConsoleColor.DarkRed;

        public Application(Window mainWindow) : base(
            System.Console.WindowWidth, System.Console.WindowHeight)
        {
            this.mainWindow = mainWindow;
            this.mainWindow.IsMainWindow = true;
            this.mainWindow.Visible = true;
            this.mainWindow.Size = new Size(Graphics.Width, Graphics.Height);
            this.mainWindow.Focus();

            System.Console.BackgroundColor = this.mainWindow.BackgroundColor;
        }

        protected override void Draw(AppTime appTime)
        {
            // main window cannot be resized by the Size property
            // only actually resizing the console will do so.
            if (System.Console.WindowHeight != mainWindow.Size.Height
                || System.Console.WindowWidth != mainWindow.Size.Width)
            {
                mainWindow.Size = new Size(System.Console.WindowWidth, System.Console.WindowHeight);
                Graphics.Resize(mainWindow.Size.Width, mainWindow.Size.Height);
                System.Console.CursorVisible = false;
            }

            WindowManager.Draw(Graphics, appTime);
        }

        protected override void Update(AppTime appTime)
        {
            WindowManager.Update(appTime);
        }

        public override void OnKeyDown(KeyInfo keyInfo)
        {
            WindowManager.OnKeyDown(keyInfo);
        }

        public static Application Current { get; private set; }

        /// <summary>
        /// Begins running a standard application message loop on the current thread, and makes the specifid form visible
        /// </summary>
        /// <param name="mainWindow"></param>
        public static void Run(Window mainWindow, Action onReady = null)
        {
            if (Current != null)
            {
                throw new ApplicationException("An application is already running!");
            }

            Current = new Application(mainWindow);
            Current.Run();

            if (onReady != null)
            {
                onReady();
            }

            while (true)
            {
                Current.OnKeyDown(new KeyInfo(System.Console.ReadKey(true)));
            }
        }

        public static void Exit()
        {
            System.Environment.Exit(0);
        }
    }
}