using System;

namespace Shinobytes.Console.Forms.Views
{
    public class ViewTemplateControlTreeBuilder : IViewTemplateControlTreeBuilder
    {
        public ControlCollection Build(IViewTemplate template, IViewObject source)
        {
            ControlCollection controls = null;
            if (source is View view)
            {
                controls = new ControlCollection(view);

                foreach (var node in template.AllNodes())
                {
                    
                }

            }
            else if (source is ViewComponent component)
            {
                controls = new ControlCollection(component);

                foreach (var node in template.AllNodes())
                {

                }
            }
            else
            {
                throw new NotImplementedException();
            }

            return controls;
        }
    }
}