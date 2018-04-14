namespace Shinobytes.Console.Forms.Views
{
    internal class Identifier : SyntaxNode
    {
        public string Value { get; }

        public Identifier(string value)
        {
            Value = value;
        }
    }
}