namespace Shinobytes.Console.Forms.Views
{
    internal class IndexedMemberAccess : SyntaxNode
    {
        public SyntaxNode LeftNode { get; }
        public Indexer Indexer { get; }

        public IndexedMemberAccess(SyntaxNode leftNode, Indexer indexer)
        {
            LeftNode = leftNode;
            Indexer = indexer;
        }
    }
}