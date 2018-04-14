namespace Shinobytes.Console.Forms.Views
{
    internal class Number : SyntaxNode
    {
        public double Value { get; }

        public Number(double value)
        {
            Value = value;
        }
    }
}