using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    /// <summary>
    /// Manager class to help making sure that the right control gets keyboard focus
    /// </summary>
    internal class InputManager
    {
        private static Control _focusedControl;
        private static Window _focusedWindow;

        private static readonly object mutex = new object();


        public static Control ActiveControl
        {
            get
            {
                lock (mutex) return _focusedControl;
            }
        }


        public static Control ActiveWindow
        {
            get
            {
                lock (mutex) return _focusedWindow;
            }
        }

        public static void Focus(Control control)
        {
            lock (mutex)
            {

                if (control is Window window)
                {
                    if (_focusedWindow != null)
                    {
                        _focusedControl.Blur();
                    }
                    _focusedWindow = window;
                    if (_focusedWindow != null) _focusedWindow.HasFocus = true;
                }
                else
                {
                    if (_focusedControl != null)
                    {
                        _focusedControl.Blur();
                    }

                    _focusedControl = control;
                    if (_focusedControl != null) _focusedControl.HasFocus = true;
                }
            }
        }

        public static void Blur(Control control)
        {
            lock (mutex)
            {
                control.HasFocus = false;
                if (control is Window window)
                {
                    if (_focusedWindow == window)
                    {
                        _focusedWindow = null;
                    }
                }
                else
                {                    
                    if (_focusedControl == control)
                    {
                        _focusedControl = null;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Manager class to keep all active windows updated and rendered
    /// </summary>
    internal class WindowManager
    {
        private static readonly List<Window> windows = new List<Window>();
        private static readonly object mutex = new object();

        public static void Register(Window window)
        {
            lock (mutex) windows.Add(window);
        }

        public static void Draw(ConsoleGraphics graphics, AppTime appTime)
        {
            lock (mutex) windows
                 .Where(x => x.Visible)
                .ForEach(x => x.Draw(graphics, appTime));
        }

        public static void Update(AppTime appTime)
        {
            lock (mutex) windows
                 .Where(x => x.IsEnabled)
                .ForEach(x => x.Update(appTime));
        }

        public static void OnKeyDown(KeyInfo keyInfo)
        {
            lock (mutex) windows
                    .Where(x => x.HasFocus && !x.EventBlocked())
                    .DoWhile(x => x.OnKeyDown(keyInfo));
        }

        public static void Unregister(Window window)
        {
            lock (mutex) windows.Remove(window);
        }
    }
}