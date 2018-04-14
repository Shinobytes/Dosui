namespace Shinobytes.Console.Forms.Views
{
    public interface IViewTemplateParser
    {
        IViewTemplate Parse(string templateContent);
    }
}