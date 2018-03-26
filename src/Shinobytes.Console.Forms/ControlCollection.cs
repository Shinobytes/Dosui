using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Shinobytes.Console.Forms
{
    public class ControlCollection<T> : IList<T> where T : Control
    {
        private readonly Control parent;
        private readonly List<T> list = new List<T>();

        public ControlCollection(Control parent)
        {
            this.parent = parent;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            item.Parent = parent;

            if (item.TransparentBackground)
            {
                item.BackgroundColor = parent.BackgroundColor;
            }

            if (item.TabIndex <= 0 && this.list.Count > 0)
            {
                item.TabIndex = this.list.Max(x => x.TabIndex) + 1;
            }

            list.Add(item);
        }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            var remove = list.Remove(item);
            if (remove)
            {
                item.Parent = null;
            }
            return remove;
        }

        public int Count => list.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            item.Parent = parent;
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this[index].Parent = null;
            list.RemoveAt(index);
        }

        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
        }
    }

    public class ControlCollection : ControlCollection<Control>
    {
        public ControlCollection(ContainerControl parent) : base(parent)
        {
        }


        public T ElementAt<T>(int index) where T : Control
        {
            return (T)this[index];
        }

    }
}