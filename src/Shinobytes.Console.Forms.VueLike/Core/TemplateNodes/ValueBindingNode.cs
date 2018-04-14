namespace Shinobytes.Console.Forms.Views
{
    internal class ValueBindingNode : IViewTemplateNode
    {
        public IViewTemplateNodeExpression BindingExpression { get; }

        public ValueBindingNode(IViewTemplateNodeExpression bindingExpression)
        {
            BindingExpression = bindingExpression;
        }
    }
}