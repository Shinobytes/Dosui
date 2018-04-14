namespace Shinobytes.Console.Forms.Views
{
    public interface IViewTemplateControlTreeBuilder
    {
        ControlCollection Build(IViewTemplate template);
    }
}