using System;
using System.Collections.Generic;
using System.IO;

namespace Shinobytes.Console.Forms.Views
{
    public class View : Window, IView
    {
        public static IViewTemplateParser TemplateParser { get; set; } = new ViewTemplateParser(new ViewTemplateExpressionParser());
        public static IViewTemplateControlTreeBuilder ControlTreeBuilder { get; set; } = new ViewTemplateControlTreeBuilder();
        public static IViewControlBinder ControlBinder { get; set; } = new ViewControlBinder();

        public readonly ViewComponentCollection Components = new ViewComponentCollection();

        public static View Instance { get; private set; }
        public ViewOptions Options { get; }
        public IViewTemplate Template { get; set; }

        public View(ViewOptions options)
        {
            //if (Instance != null)
            //{
            //    throw new NotSupportedException("Only one running instance of the type '" + nameof(View) + "' is allowed.");
            //}

            var templateContent = !string.IsNullOrEmpty(options.TemplateData)
                ? options.TemplateData
                : System.IO.File.ReadAllText(options.TemplateFile);

            Template = TemplateParser.Parse(templateContent);
            Options = options;
            Instance = this;
        }

        public static ViewComponent Component(string name, ViewComponent component)
        {
            var fullTemplateFilePath = Path.Combine(Instance.Options.ComponentRootFolder, name + ".view");
            component.Name = name;
            component.TemplateFile = fullTemplateFilePath;
            component.Template =
                TemplateParser.Parse(File.ReadAllText(fullTemplateFilePath));

            return component;
        }

        public static void Run(View viewApp)
        {
            Application.Run(viewApp, viewApp.BuildViews);
        }

        public void BuildViews()
        {
            var appControls = ControlTreeBuilder.Build(this.Template, this);
        }
    }
}