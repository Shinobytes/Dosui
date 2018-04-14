using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shinobytes.Console.Forms.Views.Tests
{
    [TestClass]
    public class View_TemplateParser_Tests
    {

        [TestMethod]
        public void ParseSimpleTemplate_UsingProperty()
        {
            var viewApp = new View(
                new ViewOptions
                {
                    Data = new
                    {
                        Hello = "Test"
                    },
                    TemplateData = "<app>{{ Hello }}</app>"
                });
        }

        [TestMethod]
        public void ParseSimpleTemplate_UsingProperty_mixed()
        {
            var viewApp = new View(
                new ViewOptions
                {
                    Data = new
                    {
                        Hello = "World"
                    },
                    TemplateData = "<app>Hello {{ Hello }}!</app>"
                });
        }

        [TestMethod]
        public void ParseTemplateNoRoot()
        {
            var viewApp = new View(
                new ViewOptions
                {
                    Data = new
                    {
                        Hello = "Test"
                    },
                    TemplateData = "{{ Hello.Length.Blabla }}"
                });
        }


        [TestMethod]
        public void ParseTemplateNoRoot_Invocation()
        {
            var viewApp = new View(
                new ViewOptions
                {
                    Data = new
                    {
                        Hello = "Test"
                    },
                    TemplateData = "{{ Hello.Length.Blabla() }}"
                });
        }

        [TestMethod]
        public void ParseTemplateNoRoot_InvocationParam()
        {
            var viewApp = new View(
                new ViewOptions
                {
                    Data = new
                    {
                        Hello = "Test"
                    },
                    TemplateData = "{{ Hello.Length.Blabla(true, false) }}"
                });
        }
    }

    [TestClass]
    public class View_TemplateControlBuilder_Tests
    {
        [TestMethod]
        public void ParseSimpleTemplate_UsingProperty()
        {
            var viewApp = new View(
                new ViewOptions
                {
                    Data = new
                    {
                        Hello = "Test"
                    },
                    TemplateData = "<app>{{ Hello }}</app>"
                });

            viewApp.BuildViews();
        }
    }
}
