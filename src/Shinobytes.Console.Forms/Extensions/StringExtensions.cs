using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shinobytes.Console.Forms.Extensions
{
    public static class StringExtensions
    {
        public static string[] SplitAt(this string str, int index)
        {
            return new[]
            {
                str.Remove(index),
                str.Substring(index)
            };
        }
    }
}
