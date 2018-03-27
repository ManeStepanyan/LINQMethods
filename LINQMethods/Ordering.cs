using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQMethods
{ /// <summary>
/// Delegate to keep the result of Compare method( positive in case of ascending, and negative otherwise)
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <param name="key1">first element to compare </param>
/// <param name="key2"> second element to compare</param>
/// <returns> nummber indicating the result of comparison </returns>
    public delegate int MyCompare<TKey>(TKey key1, TKey key2);
    public class Ordering<TSource, TKey> : IOrderedEnumerable<TSource>
    {
        private IEnumerable<TSource> source;
        private Func<TSource, TKey> keySelector;
        private bool descending;
        private IComparer<TKey> comparer;
        private List<TSource> result; // Ordered elements
        private MyCompare<TKey> COMPARE;

        public Ordering(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            this.source = source;
            this.keySelector = keySelector;
            this.comparer = comparer;
            if (!descending)
            {
                COMPARE = CompareForAscending;
            }
            else { COMPARE = CompareForDescending; }
            Sort(source, keySelector);
        }
        /// <summary>
        /// Choosing result of comparison in case of Ascending
        /// </summary>
        /// <param name="x">first element </param>
        /// <param name="y"> second element </param>
        /// <returns>number indicating the result of comparison </returns>
        public int CompareForAscending(TKey x, TKey y)
        {
            return this.comparer.Compare(x, y);
        }
        /// <summary>
        /// Choosing result of comparison in case of Descending (negating result of Ascending case)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int CompareForDescending(TKey x, TKey y)
        {
            return -(this.comparer.Compare(x, y));
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            return result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// Sorting
        /// </summary>
        /// <param name="source"> sequence to sort </param>
        /// <param name="keySelector"> key selector method </param>
        public void Sort(IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            result = new List<TSource>();
            List<KeyValuePair<TKey, TSource>> list = new List<KeyValuePair<TKey, TSource>>();
            foreach (var item in source)
            {
                list.Add(new KeyValuePair<TKey, TSource>(keySelector(item), item));
            }
            list = MergeSort(list);
            foreach (var item in list)
            {
                result.Add(item.Value);
            }

        }
        /// <summary>
        /// Applying Merge_Sort Algorithm
        /// </summary>
        /// <param name="unsorted"></param>
        /// <returns></returns>
        private List<KeyValuePair<TKey, TSource>> MergeSort(List<KeyValuePair<TKey, TSource>> unsorted)
        {
            if (unsorted.Count <= 1)
                return unsorted;

            List<KeyValuePair<TKey, TSource>> left = new List<KeyValuePair<TKey, TSource>>();
            List<KeyValuePair<TKey, TSource>> right = new List<KeyValuePair<TKey, TSource>>();

            int middle = unsorted.Count / 2;
            for (int i = 0; i < middle; i++)  //Dividing the unsorted list
            {
                left.Add(unsorted[i]);
            }
            for (int i = middle; i < unsorted.Count; i++)
            {
                right.Add(unsorted[i]);
            }

            left = MergeSort(left);
            right = MergeSort(right);
            return Merge(left, right);
        }

        private List<KeyValuePair<TKey, TSource>> Merge(List<KeyValuePair<TKey, TSource>> left, List<KeyValuePair<TKey, TSource>> right)
        {
            List<KeyValuePair<TKey, TSource>> result = new List<KeyValuePair<TKey, TSource>>();

            while (left.Count > 0 || right.Count > 0)
            {
                if (left.Count > 0 && right.Count > 0)
                {//(this.comparer.Compare(left.First().Key,right.First().Key)<=0)
                    if (this.COMPARE(left.First().Key, right.First().Key) <= 0)  //Comparing First two elements to see which is smaller
                    {
                        result.Add(left.First());
                        left.Remove(left.First());      //Rest of the list minus the first element
                    }
                    else
                    {
                        result.Add(right.First());
                        right.Remove(right.First());
                    }
                }
                else if (left.Count > 0)
                {
                    result.Add(left.First());
                    left.Remove(left.First());
                }
                else if (right.Count > 0)
                {
                    result.Add(right.First());

                    right.Remove(right.First());
                }
            }
            return result;
        }

        public IOrderedEnumerable<TSource> CreateOrderedEnumerable<TKey>(Func<TSource, TKey> keySelector, IComparer<TKey> comparer, bool descending)
        {
            return new Ordering<TSource, TKey>(source, keySelector, comparer, descending);
        }


    }
}
