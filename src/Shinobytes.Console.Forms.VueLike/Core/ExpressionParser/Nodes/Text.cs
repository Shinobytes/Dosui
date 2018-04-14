namespace Shinobytes.Console.Forms.Views
{
    internal class Text : SyntaxNode
    {
        public string Value { get; }

        public Text(string value)
        {
            Value = value;
        }
    }
}