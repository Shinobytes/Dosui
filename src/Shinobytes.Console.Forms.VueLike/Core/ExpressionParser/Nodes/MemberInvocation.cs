namespace Shinobytes.Console.Forms.Views
{
    internal class MemberInvocation : SyntaxNode
    {
        public SyntaxNode Member { get; }
        public SyntaxNode[] Arguments { get; }

        public MemberInvocation(SyntaxNode member, SyntaxNode[] arguments)
        {
            Member = member;
            Arguments = arguments;
        }
    }
}