using System.Collections;
using System.Collections.Generic;

namespace Shinobytes.Console.Forms.Views
{
    public class ViewComponentCollection : IList<ViewComponent>
    {
        private readonly List<ViewComponent> source;

        public ViewComponentCollection()
        {
            this.source = new List<ViewComponent>();
        }
        public IEnumerator<ViewComponent> GetEnumerator()
        {
            return source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ViewComponent item)
        {
            source.Add(item);
        }

        public void Clear()
        {
            source.Clear();
        }

        public bool Contains(ViewComponent item)
        {
            return source.Contains(item);
        }

        public void CopyTo(ViewComponent[] array, int arrayIndex)
        {
            source.CopyTo(array, arrayIndex);
        }

        public bool Remove(ViewComponent item)
        {
            return source.Remove(item);
        }

        public int Count => source.Count;

        public bool IsReadOnly => false;

        public int IndexOf(ViewComponent item)
        {
            return source.IndexOf(item);
        }

        public void Insert(int index, ViewComponent item)
        {
            source.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            source.RemoveAt(index);
        }

        public ViewComponent this[int index]
        {
            get => source[index];
            set => source[index] = value;
        }
    }
}