namespace Shinobytes.Console.Forms.Views
{
    internal struct Token
    {
        public Token(string value, TokenType type)
        {
            Value = value;
            Type = type;
        }

        public string Value { get; }
        public TokenType Type { get; }
    }
}