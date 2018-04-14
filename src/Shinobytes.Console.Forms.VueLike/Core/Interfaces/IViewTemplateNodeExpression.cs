using System.Collections.Generic;
using System.Linq.Expressions;

namespace Shinobytes.Console.Forms.Views
{
    public interface IViewTemplateNodeExpression : IViewTemplateNode
    {
        bool Compile(IViewTemplateNodeExpressionCompiler compiler);
    }

    public interface IViewTemplateNodeExpressionCompiler
    {
        Expression Compile(IReadOnlyList<SyntaxNode> nodes);
    }

    public class ViewTemplateNodeExpression : IViewTemplateNodeExpression
    {
        private readonly IReadOnlyList<SyntaxNode> nodes;
        private Expression expression;

        internal ViewTemplateNodeExpression(IReadOnlyList<SyntaxNode> nodes)
        {
            this.nodes = nodes;
        }

        public bool Compile(IViewTemplateNodeExpressionCompiler compiler)
        {
            try
            {
                this.expression = compiler.Compile(this.nodes);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}