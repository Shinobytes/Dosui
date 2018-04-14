namespace Shinobytes.Console.Forms.Views
{
    internal class MemberAccess : SyntaxNode
    {
        public SyntaxNode LeftNode { get; }
        public SyntaxNode RightNode { get; }

        public MemberAccess(SyntaxNode leftNode, SyntaxNode rightNode)
        {
            LeftNode = leftNode;
            RightNode = rightNode;
        }
    }
}