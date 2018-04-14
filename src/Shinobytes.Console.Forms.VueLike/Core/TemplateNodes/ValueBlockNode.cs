namespace Shinobytes.Console.Forms.Views
{
    internal class ValueBlockNode : IViewTemplateNode
    {
        public IViewTemplateNode[] Children { get; }

        public ValueBlockNode(params IViewTemplateNode[] children)
        {
            Children = children;
        }
    }

    internal class BlockNode : IViewTemplateNode
    {
        public IViewTemplateNode[] Children { get; }

        public BlockNode(params IViewTemplateNode[] children)
        {
            Children = children;
        }
    }
}