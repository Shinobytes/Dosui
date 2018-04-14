namespace Shinobytes.Console.Forms.Views
{
    internal interface IViewTemplateParser
    {
        IViewTemplate Parse(string templateContent);
    }
}