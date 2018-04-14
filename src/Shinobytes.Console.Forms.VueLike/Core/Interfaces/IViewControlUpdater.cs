namespace Shinobytes.Console.Forms.Views
{
    public interface IViewControlUpdater
    {
        void Update(View view);
        void Update(ViewComponent component);
    }
}