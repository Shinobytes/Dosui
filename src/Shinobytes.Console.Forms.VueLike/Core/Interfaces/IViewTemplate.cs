using System.Collections.Generic;

namespace Shinobytes.Console.Forms.Views
{
    public interface IViewTemplate
    {        
        IReadOnlyList<IViewTemplateNode> AllNodes();
    }
}