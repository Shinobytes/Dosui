namespace Shinobytes.Console.Forms.Views
{
    internal class Indexer : SyntaxNode
    {
        public SyntaxNode[] IndexerExpressions { get; }

        public Indexer(params SyntaxNode[] indexerExpressions)
        {
            IndexerExpressions = indexerExpressions;
        }
    }
}