using System;
using System.Collections.Generic;

namespace Shinobytes.Console.Forms
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> body)
        {
            foreach (var item in items)
            {
                body(item);
            }
        }

        public static bool DoWhile<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            foreach (var item in items)
            {
                if (!predicate(item))
                {
                    return false;
                }
            }
            return true;
        }

        public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            var index = 0;
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    return index;
                }

                index++;
            }
            return -1;
        }
    }
}