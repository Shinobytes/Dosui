namespace Shinobytes.Console.Forms.Views
{
    public abstract class ViewComponent : ContainerControl, IViewComponent
    {
        public string Name { get; set; }
        public string TemplateFile { get; set; }
        public IViewTemplate Template { get; set; }
        public abstract object Data();
    }
}