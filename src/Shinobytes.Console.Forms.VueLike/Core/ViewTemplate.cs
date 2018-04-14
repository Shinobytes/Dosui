using System.Collections.Generic;

namespace Shinobytes.Console.Forms.Views
{
    internal class ViewTemplate : IViewTemplate
    {
        private readonly IReadOnlyList<IViewTemplateNode> controls;

        public ViewTemplate(IReadOnlyList<IViewTemplateNode> controls)
        {
            this.controls = controls;
        }

        public IReadOnlyList<IViewTemplateNode> AllNodes()
        {
            return controls;
        }
    }
}