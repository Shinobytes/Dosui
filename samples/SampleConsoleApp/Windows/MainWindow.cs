using System;
using System.Drawing;
using Shinobytes.Console.Forms;

namespace SampleConsoleApp
{
    public class MainWindow : Window
    {
        private readonly Window aboutWindow;
        private readonly Window testWindow;
        private StatusStrip statusStrip;

        public MainWindow()
        {
            this.Text = "Test";

            aboutWindow = new AboutWindow();
            testWindow = new TestConfirmationWindow();
            //testWindow.Closed += (sender, result) =>
            //{
            //    var value = result.GetValueOrDefault();
            //};

            AddInfoLabel();

            var pb = new ProgressBar
            {
                Position = new Point(50, 2),
                Size = new Size(35, 1),
                Value = 60.5f
            };

            var toggle = new ToggleButton
            {
                Position = new Point(1, 2),
                Text = "Toggle me by pressing Enter"
            };
            toggle.Focus();
            this.Controls.Add(toggle);

            var toggle2 = new ToggleButton
            {
                Position = new Point(1, 4),
                Text = "Or by pressing the spacebar!"
            };
            this.Controls.Add(toggle2);

            var toggle3 = new ToggleButton
            {
                Position = new Point(1, 5),
                Text = "Indeterminate progressbar"
            };
            toggle3.Invoke += (sender, args) =>
            {
                pb.Indeterminate = toggle3.IsChecked;
            };
            this.Controls.Add(toggle3);


            // pb.Indeterminate = true;
            // pb.ProgressValuePosition = ProgressValuePosition.Above;
            // pb.ProgressBackColor = this.BackgroundColor; // << for "transparent" background
            this.Controls.Add(pb);


            var btn3 = new Button
            {
                Position = new Point(2, 10),
                Size = new Size(20, 1),
                Text = "Not fancy button",
                BackgroundColor = ConsoleColor.Blue,
                ForegroundColor = ConsoleColor.White,
                DropShadow = false
            };
            this.Controls.Add(btn3);


            var btn1 = new Button
            {
                Position = new Point(25, 10),
                Size = new Size(13, 1),
                Text = "Press me!"
            };
            this.Controls.Add(btn1);

            var btn2 = new Button
            {
                Position = new Point(50, 10),
                Size = new Size(11, 1),
                Text = "And me!"
            };
            btn2.Invoke += (sender, args) =>
            {
                btn1.Text = "Now me please!";
                btn1.Size = new Size(18, 1);
            };
            this.Controls.Add(btn2);

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
                menuStrip.Controls.Add(fileMenu);
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
                    IsEnabled = false
                });
                editMenu.SubItems.Add(new MenuItem("Delete"));
                menuStrip.Controls.Add(editMenu);
            }

            var viewMenu = new MenuItem("&View");
            {
                viewMenu.SubItems.Add(new MenuItem("&Code"));
                viewMenu.SubItems.Add(new MenuItem("&Magic"));
                menuStrip.Controls.Add(viewMenu);
            }

            var toolsMenu = new MenuItem("&Tools");
            {
                toolsMenu.SubItems.Add(new MenuItem("&Customize..."));
                toolsMenu.SubItems.Add(new MenuItem("&Options"));
                menuStrip.Controls.Add(toolsMenu);
            }

            var helpMenu = new MenuItem("&Help");
            {
                var about = new MenuItem("&About");
                about.Invoke += (sender, args) => { aboutWindow.Show(); };

                helpMenu.SubItems.Add(new MenuItem("Send &feedback"));
                helpMenu.SubItems.Add(new SeparatorMenuItem());
                helpMenu.SubItems.Add(about);
            }

            menuStrip.Controls.Add(helpMenu);
            this.Controls.Add(menuStrip);


            statusStrip = new StatusStrip();
            statusStrip.Controls.Add(new TextBlock("F1=About")
            {
                ForegroundColor = ConsoleColor.Black
            });
            statusStrip.Controls.Add(new TextBlock("F2=Bananas")
            {
                ForegroundColor = ConsoleColor.Black
            });
            statusStrip.Controls.Add(new TextBlock("F3=Hax")
            {
                ForegroundColor = ConsoleColor.Black
            });
            statusStrip.Controls.Add(new TextBlock("F4=Open file dialog")
            {
                ForegroundColor = ConsoleColor.Black
            });
            this.Controls.Add(statusStrip);

        }

        private void AddInfoLabel()
        {
            var label = new TextBlock(
                " Press 'TAB' anytime you want to focus on a different control. And sometimes even the Arrow Keys can be used as long as the active/focused control does not listen for those particular keys." +
                "                                                                                                                                        " +
                "                                                                                                                                        " +
                "                                                                                                                                        " +
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
                new Point(1, 7));

            this.Controls.Add(label);
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            if (key.Key == ConsoleKey.F3)
            {
                MessageBox.Show(
                    "Thank you for letting me hax your system folder by putting 3 gb \nof pictures with Nicholas Cage",
                    " Test ",
                    MessageBoxButtons.OKCancel, res =>
                {

                });
            }
            if (key.Key == ConsoleKey.F4)
            {
                var ofd = new OpenFileDialog();
                ofd.Show(result =>
                {
                    if (result.GetValueOrDefault())
                    {
                        MessageBox.Show(
                            "So you want to open: " + ofd.SelectedFile + ", eh?",
                            " File to be opened ",
                            MessageBoxButtons.OK);
                    }
                    else
                    {
                        MessageBox.Show("Damn you're grumpy, son!", " No file? Fine! ", MessageBoxButtons.OK);
                    }
                });
            }

            if (key.Key == ConsoleKey.F1)
            {
                aboutWindow.Show();
            }

            if (key.Key == ConsoleKey.F2)
            {
                testWindow.Show(res =>
                {
                    var text = this.Controls.FindOfType<TextBlock>();
                    if (text)
                    {
                        text.Text = " " + res + " " + text.Text;
                    }
                });
            }

            return base.OnKeyDown(key);
        }
    }
}