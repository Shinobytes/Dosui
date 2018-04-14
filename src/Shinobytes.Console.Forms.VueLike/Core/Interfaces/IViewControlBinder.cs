namespace Shinobytes.Console.Forms.Views
{
    public interface IViewControlBinder
    {
        void Update(View view);
        void Update(ViewComponent component);
    }
}