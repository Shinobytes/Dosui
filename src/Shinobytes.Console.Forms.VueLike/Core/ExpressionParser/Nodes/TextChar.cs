namespace Shinobytes.Console.Forms.Views
{
    internal class TextChar : SyntaxNode
    {
        public char Value { get; }

        public TextChar(char value)
        {
            Value = value;
        }
    }
}