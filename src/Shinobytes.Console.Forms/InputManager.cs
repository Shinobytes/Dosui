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
                    if (_focusedWindow && window != _focusedWindow)
                    {
                        _focusedWindow.Blur();
                    }
                    _focusedWindow = window;
                    if (_focusedWindow)
                    {
                        _focusedWindow.HasFocus = true;
                    }
                }
                else
                {
                    if (_focusedControl)
                    {
                        _focusedControl.Blur();
                    }

                    _focusedControl = control;
                    if (_focusedControl) _focusedControl.HasFocus = true;
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
                        var mainWindow = WindowManager.MainWindow;
                        if (window.Parent)
                        {
                            window.Parent.HasFocus = true;
                            _focusedWindow = window.Parent as Window;
                        }
                        else if (mainWindow && mainWindow != window)
                        {
                            mainWindow.HasFocus = true;
                            _focusedWindow = mainWindow;
                        }
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
}