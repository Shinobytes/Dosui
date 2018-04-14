namespace Shinobytes.Console.Forms.Views
{
    public abstract class ViewComponent
    {
        public string Name { get; set; }        
        public string TemplateFile { get; set; }
        public IViewTemplate Template { get; set; }
        public abstract object Data();
    }
}