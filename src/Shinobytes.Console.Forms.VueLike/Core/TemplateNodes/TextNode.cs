namespace Shinobytes.Console.Forms.Views
{
    internal class TextNode : IViewTemplateNode
    {
        public string Text { get; }

        public TextNode(string text)
        {
            Text = text;
        }
    }
}