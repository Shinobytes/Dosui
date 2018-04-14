using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace Shinobytes.Console.Forms.Views
{
    internal class ViewTemplateParser : IViewTemplateParser
    {
        private readonly IViewTemplateExpressionParser expressionParser;

        internal ViewTemplateParser(IViewTemplateExpressionParser expressionParser)
        {
            this.expressionParser = expressionParser;
        }

        public IViewTemplate Parse(string templateContent)
        {
            var doc = Load(templateContent);
            var controls = new List<IViewTemplateNode>();

            foreach (var element in doc.DocumentNode.ChildNodes)
            {
                var control = ParseTemplateControl(element);
                if (control != null)
                {
                    controls.Add(control);
                }
            }

            return new ViewTemplate(controls);
        }

        private IViewTemplateNode ParseTemplateControl(HtmlNode element)
        {
            if (element is HtmlTextNode text)
            {
                var textContent = text.InnerHtml;
                var finalContent = "";
                if (textContent.Contains("{"))
                {
                    var regex = new Regex("({{.*}})");
                    var matches = regex.Matches(textContent);
                    var code = matches[0].Value;

                    var leftSide = textContent.Split('{').FirstOrDefault();
                    var centerExpression = expressionParser.Parse(code);
                    var rightSide = textContent.Substring(textContent.LastIndexOf("}"));
                    return new ValueBlockNode(
                        new TextNode(leftSide),
                        new ValueBindingNode(centerExpression),
                        new TextNode(rightSide)
                    );
                }
                return ParseText(textContent);
            }

            return ParseNode(element);
        }

        private IViewTemplateNode ParseNode(HtmlNode element)
        {
            var nodes = new List<IViewTemplateNode>();
            if (element.HasChildNodes)
            {
                foreach (var node in element.ChildNodes)
                {
                    var value = ParseTemplateControl(node);
                    if (value != null)
                    {
                        nodes.Add(value);
                    }
                }
            }

            if (nodes.Count > 1)
            {
                return new BlockNode(nodes.ToArray());
            }

            return nodes.FirstOrDefault();
        }

        private IViewTemplateNode ParseText(string text)
        {
            return new TextNode(text);
        }

        private static HtmlDocument Load(string templateContent)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(templateContent);
            return doc;
        }
    }
}