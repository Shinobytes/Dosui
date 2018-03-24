using System;
using System.Drawing;
using Shinobytes.Console.Forms;

namespace SampleConsoleApp
{
    public class MainWindow : Window
    {
        private readonly Window aboutWindow;

        public MainWindow()
        {
            this.Text = "Test";

            var label = new TextBlock(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborumLorem ipsum dolor sit amet, " +
                "consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum" +
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborumLorem ipsum dolor sit amet, " +
                "consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
                "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
                "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum",
                new Point(1, 1));

            this.Controls.Add(label);

            var menuStrip = new MenuStrip();

            var fileMenu = new MenuItem("&File");
            {
                fileMenu.SubItems.Add(new MenuItem("&New"));
                fileMenu.SubItems.Add(new MenuItem("&Open"));
                fileMenu.SubItems.Add(new SeparatorMenuItem());
                fileMenu.SubItems.Add(new MenuItem("&Save"));
                fileMenu.SubItems.Add(new MenuItem("Save &as..."));
                fileMenu.SubItems.Add(new MenuItem("Save a&ll"));
                fileMenu.SubItems.Add(new SeparatorMenuItem());
                var mi = new MenuItem("E&xit");
                mi.Invoke += (sender, args) =>
                {
                    Application.Exit();
                };

                fileMenu.SubItems.Add(mi);
                menuStrip.Items.Add(fileMenu);
            }

            var editMenu = new MenuItem("&Edit");
            {
                editMenu.SubItems.Add(new MenuItem("Undo"));
                editMenu.SubItems.Add(new MenuItem("Redo"));
                editMenu.SubItems.Add(new SeparatorMenuItem());
                editMenu.SubItems.Add(new MenuItem("Cut"));
                editMenu.SubItems.Add(new MenuItem("Copy"));
                editMenu.SubItems.Add(new MenuItem("Paste")
                {
                    Enabled = false
                });
                editMenu.SubItems.Add(new MenuItem("Delete"));
                menuStrip.Items.Add(editMenu);
            }

            var viewMenu = new MenuItem("&View");
            {
                viewMenu.SubItems.Add(new MenuItem("&Code"));
                viewMenu.SubItems.Add(new MenuItem("&Magic"));
                menuStrip.Items.Add(viewMenu);
            }

            var toolsMenu = new MenuItem("&Tools");
            {
                toolsMenu.SubItems.Add(new MenuItem("&Customize..."));
                toolsMenu.SubItems.Add(new MenuItem("&Options"));
                menuStrip.Items.Add(toolsMenu);
            }

            var helpMenu = new MenuItem("&Help");
            {
                aboutWindow = new Window
                {
                    BackgroundColor = ConsoleColor.Gray,
                    Text = " About ",
                    Size = new Size(40, 16),
                    Position = new Point(40, 5)
                };

                var title = "Shinobytes.Console.Forms";
                aboutWindow.Controls.Add(new TextBlock(title, ConsoleColor.Red, new Point(6, 1)));

                aboutWindow.Controls.Add(new TextBlock(
                    new String(AsciiCodes.BorderDouble_Horizontal, title.Length), ConsoleColor.DarkGray, new Point(6, 2)));

                aboutWindow.Controls.Add(new TextBlock("Version 1.0", ConsoleColor.Black, new Point(12, 3)));
                aboutWindow.Controls.Add(new TextBlock("by", ConsoleColor.Black, new Point(16, 5)));
                aboutWindow.Controls.Add(new TextBlock("zerratar@gmail.com", ConsoleColor.DarkCyan, new Point(8, 6)));

                aboutWindow.Controls.Add(new TextBlock(
                    AsciiCodes.BorderSingle_SplitToRight +
                    new String(AsciiCodes.BorderSingle_Horizontal, aboutWindow.Size.Width - 4) +
                    AsciiCodes.BorderSingle_SplitToLeft, ConsoleColor.DarkGray, new Point(-1, 8)));

                aboutWindow.Controls.Add(new TextBlock("This software is licensed", ConsoleColor.DarkGray, new Point(4, 10)));
                aboutWindow.Controls.Add(new TextBlock("under MIT license so you", ConsoleColor.DarkGray, new Point(5, 11)));
                aboutWindow.Controls.Add(new TextBlock("can do whatever you want with it.", ConsoleColor.DarkGray, new Point(2, 12)));

                var about = new MenuItem("&About");
                about.Invoke += (sender, args) => { aboutWindow.Show(); };

                helpMenu.SubItems.Add(new MenuItem("Send &feedback"));
                helpMenu.SubItems.Add(new SeparatorMenuItem());
                helpMenu.SubItems.Add(about);
            }

            menuStrip.Items.Add(helpMenu);
            this.Controls.Add(menuStrip);
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            if (key.Key == ConsoleKey.Spacebar)
            {
                if (aboutWindow.Visible)
                {
                    aboutWindow.Hide();
                }
                else
                {
                    aboutWindow.Show();
                }
            }
            return base.OnKeyDown(key);
        }
    }
}