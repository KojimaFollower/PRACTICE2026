using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace task03
{
    public class CustomCollection<T> : IEnumerable<T>
    {
        private readonly List<T> _items;

        public CustomCollection(List<T> numbers)
        {
            ArgumentNullException.ThrowIfNull(numbers);

            _items = new List<T>(numbers);
        }

        public CustomCollection()
        {
            _items = new List<T>();
        }

        public IEnumerable<T> GetReverseEnumerator()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
        }

        public static IEnumerable<int> GenerateSequence(int start, int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Количество элементов не может быть меньше нуля");
            }

            for (int i = 0; i < count; i++)
            {
                yield return start + i;
            }
        }

        public IEnumerable<T> FilterAndSort(Func<T, bool> predicate, Func<T, IComparable> keySelector)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            ArgumentNullException.ThrowIfNull(keySelector);
            return _items.Where(predicate).OrderBy(keySelector);
        }

        public void Add(T item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            _items.Add(item);
        }

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

