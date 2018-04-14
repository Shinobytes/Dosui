namespace Shinobytes.Console.Forms.Views
{
    internal interface IViewTemplateExpressionParser
    {
        IViewTemplateNodeExpression Parse(string textContent);
    }
}