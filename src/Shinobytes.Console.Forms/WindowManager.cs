using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    /// <summary>
    /// Manager class to keep all active windows updated and rendered
    /// </summary>
    internal class WindowManager
    {
        private static readonly List<Window> windows = new List<Window>();
        private static readonly object mutex = new object();

        public static Window MainWindow
        {
            get
            {
                lock (mutex)
                {
                    return windows.FirstOrDefault(x => x.IsMainWindow);
                }
            }
        }

        public static bool Register(Window window)
        {
            lock (mutex)
            {
                if (windows.Contains(window))
                    return false;

                windows.Add(window);
                return true;
            }
        }

        public static void Draw(ConsoleGraphics graphics, AppTime appTime)
        {
            lock (mutex) windows
                 .Where(x => x.Visible).ToList()
                .ForEach(x => x.Draw(graphics, appTime));
        }

        public static void Update(AppTime appTime)
        {
            lock (mutex)
            {
                windows.Where(x => x.IsEnabled).ToList().ForEach(x => x.Update(appTime));
            }
        }

        public static void OnKeyDown(KeyInfo keyInfo)
        {
            lock (mutex)
            {
                var list = windows
                    .Where(x => x.HasFocus && !x.EventBlocked())
                    .ToList();

                // dialogs steals keyfocus
                if (list.Any(x => x.IsDialog))
                {
                    list.First(x => x.IsDialog).OnKeyDown(keyInfo);
                }
                else
                {
                    list.DoWhile(x => x.OnKeyDown(keyInfo));
                }

            }
        }

        public static void Unregister(Window window)
        {
            lock (mutex)
            {
                windows.Remove(window);
            }
        }
    }
}