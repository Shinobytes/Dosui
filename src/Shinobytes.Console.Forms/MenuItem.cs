using System;
using System.Collections.Generic;
using System.Linq;
using Shinobytes.Console.Forms.Graphics;

namespace Shinobytes.Console.Forms
{
    public class MenuItem : Control
    {
        public MenuItem()
        {
            this.ShortcutListener = true;
            SubItems = new ControlCollection<MenuItem>(this);
        }

        public MenuItem(string text) : this()
        {
            Text = text;
            ForegroundColor = ConsoleColor.Black;
            BackgroundColor = ConsoleColor.Gray;

        }

        public event EventHandler Invoke;

        public ControlCollection<MenuItem> SubItems { get; }

        public ConsoleColor BorderColor { get; set; } = ConsoleColor.DarkGray;

        public int MinWidth { get; set; } = 10;

        public BorderThickness BorderThickness { get; set; } = new BorderThickness(0, 1, 1, 1);

        public bool IsMenuOpen { get; set; }

        public bool IsSelected { get; set; }


        public override void Focus()
        {
            base.Focus();
            this.IsSelected = true;
            this.IsMenuOpen = true;
        }

        public override void Blur()
        {
            base.Blur();
            this.IsMenuOpen = false;
            this.IsSelected = false;
        }

        public override void Draw(IGraphics graphics, AppTime appTime)
        {
            // todo: draw individual char, check for & and have a different color for the char thereafter
            // ex: &File, should emphasize on (F)
            var ops = ParseDrawOperations($" {this.Text} ");
            var totalWidth = 0;

            //graphics.DrawLine(Position.X, Position.Y, Position.X + Size.Width - 1, Position.Y, this.BackgroundColor);

            if (this.IsSelected)
            {
                foreach (var op in ops)
                {
                    graphics.DrawString(op.Text, Position.X + totalWidth, Position.Y, ConsoleColor.White, Application.ThemeColor);
                    totalWidth += op.Text.Length;
                }

                var remaining = this.Size.Width - 1 - totalWidth;
                if (remaining > 0)
                    graphics.DrawString(new string(' ', remaining), Position.X + totalWidth, Position.Y, ConsoleColor.White, Application.ThemeColor);
            }
            else
            {
                foreach (var op in ops)
                {
                    graphics.DrawString(
                        op.Text, Position.X + totalWidth, Position.Y,
                        this.IsEnabled ? op.ForegroundColor : this.DisabledForegroundColor, op.BackgroundColor);
                    totalWidth += op.Text.Length;
                }
            }

            if (this.IsMenuOpen)
            {
                // draw sub items, this is actually the menu, but to draw it we have to determine the amount of rows we want to draw and then 
                // draw borders around them.
                if (SubItems.Count > 0)
                {
                    var height = SubItems.Count + 3; // +2 is th border top and border bottom
                    var width = Math.Max(this.MinWidth, SubItems.Max(x => (x.Text?.Length + 2) ?? 0)) + 2; // max width
                    var offsetY = 2;
                    var borderOffsetY = 1;
                    if (BorderThickness.Top == 0)
                    {
                        --height;
                        --offsetY;
                        --borderOffsetY;
                    }

                    graphics.DrawShadowRect(this.Position.X, this.Position.Y + 1, width, height, this.BackgroundColor);
                    graphics.DrawBorder(BorderThickness, this.Position.X, this.Position.Y + borderOffsetY, width - 2, height - offsetY, this.BorderColor, this.BackgroundColor);

                    // draw each item
                    for (var i = 0; i < SubItems.Count; ++i)
                    {
                        using (var gfx = graphics.CreateViewport(this.Position.X + 1, this.Position.Y + i + offsetY))
                        {
                            SubItems[i].Size = new System.Drawing.Size(width - 2, 1);
                            SubItems[i].Draw(gfx, appTime);
                        }
                    }
                }
            }
        }


        private IReadOnlyList<DrawStringOperation> ParseDrawOperations(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new List<DrawStringOperation>();
            }

            return text.Split('&').Select((x, y) =>
                y == 0
                    ? new[] { new DrawStringOperation(x, this.ForegroundColor, this.BackgroundColor) }
                    : new[] {
                        new DrawStringOperation(x[0].ToString(), Application.ThemeColor, this.BackgroundColor),
                        new DrawStringOperation(x.Substring(1), this.ForegroundColor, this.BackgroundColor) })
                        .SelectMany(x => x)
                        .ToList();

            //// just a super simple parser/tokenizer to handle the & op, faster though
            //var list = new List<DrawStringOperation>();
            //var index = 0;
            //do
            //{
            //    var str = "";
            //    var token = text[index];
            //    switch (token)
            //    {
            //        case '&':
            //            list.Add(new DrawStringOperation(text[++index].ToString(), ThemeColor, this.BackgroundColor));
            //            break;
            //        default:
            //            str += text[index];
            //            if (index + 1 < text.Length)
            //            {
            //                var next = text[index + 1];
            //                while (index + 1 < text.Length)
            //                {
            //                    if (text[index + 1] == '&') break;
            //                    next = text[++index];
            //                    str += next;
            //                }
            //            }
            //            list.Add(new DrawStringOperation(str, this.ForegroundColor, this.BackgroundColor));
            //            break;
            //    }
            //    ++index;
            //} while (index < text.Length);
            //return list;
        }

        public override void Update(AppTime appTime)
        {
        }

        public override bool OnKeyDown(KeyInfo key)
        {
            // dont do this unless we update the text
            // but for testing purposes, it should be OKAY

            if (key.Key == ConsoleKey.Enter)
            {
                if (this.IsSelected)
                {
                    this.Invoke?.Invoke(this, EventArgs.Empty);
                    this.HideMenu();
                    return false;
                }
                else
                {
                    var selectedIndex = this.SubItems.IndexOf(x => x.IsSelected);
                    if (selectedIndex > -1)
                    {
                        this.SubItems[selectedIndex].Invoke?.Invoke(this.SubItems[selectedIndex], EventArgs.Empty);
                        this.HideMenu();
                        return false;
                    }
                }
            }

            if (this.IsMenuOpen && key.Key == ConsoleKey.LeftArrow)
            {
                if (Parent is MenuStrip menu)
                {
                    var index = menu.Controls.IndexOf(this);
                    if (index > 0)
                    {
                        menu.ControlAt<MenuItem>(index - 1).ShowMenu();
                        this.HideMenu();
                    }
                }
                else
                {
                    throw new NotImplementedException("Cannot navigate in sub menu items yet");
                }
            }
            else if (this.IsMenuOpen && key.Key == ConsoleKey.RightArrow)
            {
                if (Parent is MenuStrip menu)
                {
                    var index = menu.Controls.IndexOf(this);
                    if (index + 1 < menu.Controls.Count)
                    {
                        menu.ControlAt<MenuItem>(index + 1).ShowMenu();
                        menu.Controls[index + 1].StopNextEvent();
                        this.HideMenu();
                    }
                }
                else
                {
                    throw new NotImplementedException("Cannot navigate in sub menu items yet");
                }
            }
            else if (this.IsMenuOpen && key.Key == ConsoleKey.DownArrow)
            {
                if (Parent is MenuStrip menu)
                {
                    if (this.SubItems.Count > 0)
                    {
                        this.IsSelected = false;
                        var index = 1;
                        var selectedIndex = this.SubItems.IndexOf(x => x.IsSelected);
                        while (selectedIndex + index < this.SubItems.Count)
                        {
                            if (selectedIndex >= 0)
                            {
                                this.SubItems[selectedIndex].IsSelected = false;
                            }
                            if (this.SubItems[selectedIndex + index] is SeparatorMenuItem || !this.SubItems[selectedIndex + index].IsEnabled)
                            {
                                index++;
                                continue;
                            }
                            this.SubItems[selectedIndex + index].IsSelected = true;
                            this.SubItems[selectedIndex + index].StopNextEvent();
                            break;
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException("Cannot navigate in sub menu items yet");
                }
            }
            else if (this.IsMenuOpen && key.Key == ConsoleKey.UpArrow)
            {
                if (Parent is MenuStrip menu)
                {
                    if (this.SubItems.Count > 0)
                    {
                        var selectedIndex = this.SubItems.IndexOf(x => x.IsSelected);
                        var index = 1;
                        while (selectedIndex - index >= 0)
                        {
                            this.SubItems[selectedIndex].IsSelected = false;
                            if (this.SubItems[selectedIndex - index] is SeparatorMenuItem || !this.SubItems[selectedIndex - index].IsEnabled)
                            {
                                index++;
                                continue;
                            }
                            this.SubItems[selectedIndex - index].IsSelected = true;
                            break;
                        }
                    }
                }
                else
                {
                    throw new NotImplementedException("Cannot navigate in sub menu items yet");
                }
            }
            else
            {
                // if my menu is open, we should focus on them instead. so we can have same trigger key
                if (this.IsMenuOpen)
                {
                    MenuItem selection = null;
                    foreach (var item in this.SubItems)
                    {
                        var ops = ParseDrawOperations(item.Text);
                        var hotkeyChar = ops.FirstOrDefault(x => x.ForegroundColor != this.ForegroundColor);
                        if (hotkeyChar == null) continue;
                        if (char.ToLower(hotkeyChar.Text[0]) == char.ToLower(key.KeyChar))
                        {
                            this.IsSelected = false;
                            item.IsSelected = true;
                            selection = item;
                        }
                    }
                    if (selection != null)
                    {
                        SubItems.Where(x => x != selection).ForEach(x => x.IsSelected = false);
                        return false;
                    }
                    this.HideMenu();
                }
                else
                {
                    var ops = ParseDrawOperations(this.Text);
                    var hotkeyChar = ops.FirstOrDefault(x => x.ForegroundColor != this.ForegroundColor);
                    if (hotkeyChar == null)
                    {
                        return true;
                    }

                    if (char.ToLower(hotkeyChar.Text[0]) == char.ToLower(key.KeyChar))
                    {
                        if (this.Parent is MenuStrip strip) strip.Controls.OfType<MenuItem>().ForEach(x => x.HideMenu());
                        else if (this.Parent is MenuItem mi) mi.SubItems.ForEach(x => x.HideMenu());
                        this.ShowMenu();
                        return false; // stops other items from receiving keydown events
                    }

                }
            }

            return true;
        }

        private void ShowMenu()
        {
            this.Focus();
            this.IsSelected = true;
            this.IsMenuOpen = true;
        }

        private void HideMenu()
        {
            //this.Blur();
            this.IsSelected = false;
            this.IsMenuOpen = false;
            this.SubItems.ForEach(x => x.IsSelected = false);
        }
    }
}