using Shinobytes.Console.Forms.Views;
using VueLikeSample.Components;

namespace VueLikeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var viewApp = new View(
                new ViewOptions
                {
                    Data = new
                    {
                        Hello = "Test"
                    },
                    TemplateFile = "app.view",
                    ComponentRootFolder = "Components",
                });

            View.Component("hello-world", new HelloWorldComponent());


            // run after all initialization steps
            View.Run(viewApp);
        }
    }
}
