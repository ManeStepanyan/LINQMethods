using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQMethods
{ /// <summary>
///  Implement some LINQ methods as extension methods in LINQ style
/// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Projects each element of a sequence into a new form
        /// </summary>
        /// <typeparam name="TSource"> type of the elements from sequence </typeparam>
        /// <typeparam name="TResult"> type of the new form </typeparam>
        /// <param name="source"> sequence </param>
        /// <param name="selector"> delegate to indicate the form </param>
        /// <returns> sequence after altering it by selector </returns>
        public static IEnumerable<TResult> ExtensionSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new Exception("Source is null");
            if (selector == null) throw new Exception("Selector is null");
            return new ClassForSELECT<TSource, TResult>(source, selector);
        }
        /* Another version of implementation
         *  public static  IEnumerable<TResult> ExtensionSelect<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if(source==null || selector==null)
            {
                throw new ArgumentNullException();
            }return SelectIterator(source, selector);
           
        } 
        private static IEnumerable<TResult> SelectIterator<TSource,TResult>(IEnumerable<TSource> source, Func<TSource,TResult> selector)
        {
            foreach(TSource item in source)
                yield return selector(item);
        }
         * */
        /// <summary>
        /// Filters a sequence of values based on a predicate
        /// </summary>
        /// <typeparam name="TSource">type of the elements from sequence </typeparam>
        /// <param name="source"> sequence </param>
        /// <param name="predicate"> delegate which returns boolean value </param>
        /// <returns> Filtered sequence </returns>
        public static IEnumerable<TSource> ExtensionWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source is null");
            if (predicate == null) throw new ArgumentNullException("predicate is null");
            return new ClassForWHERE<TSource>(source, predicate);
        }
        /* Another implementation( note the implementation of IEnumerable<TSource> ExtensionWhere<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate) also
         * public static IEnumerable<TSource> ExtensionWhere<TSource>(this IEnumerable<TSource> source, Func<TSource,bool> predicate)
        {
            if(source==null || predicate==null)
            {
                throw new ArgumentNullException();
            }
           return WhereIterator(source, predicate);
        }

        private static IEnumerable<TSource> WhereIterator<TSource>(IEnumerable<TSource> source, Func<TSource,bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                    yield return item;
            }
        }
        public static IEnumerable<TSource> Where<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, int, bool> predicate)
        {
            if (source == null || predicate==null)
            {
                throw new ArgumentNullException();
            }

            return WhereIterator2(source, predicate);
        }
        private static IEnumerable<TSource> WhereIterator2<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, int, bool> predicate)
        {
            int index = 0;
            foreach (var item in source)
            {
                if (predicate(item, index))
                {
                    yield return item;
                }
                index++;
            }
        } */
        /// <summary>
        /// Creates a list from IEnumerable
        /// </summary>
        /// <typeparam name="TSource">type of the elements from sequence </typeparam>
        /// <param name="source"> sequence </param>
        /// <returns> List made from source </returns>
        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("Source is null");
            }
            return source.ToList();
        }
        /// <summary>
        /// Creates a Dictionary from an IEnumerable according to a specified key selector function.
        /// </summary>
        /// <typeparam name="TSource"> type of IEnumerable elements </typeparam>
        /// <typeparam name="TKey"> type of key </typeparam>
        /// <param name="source"> sequence </param>
        /// <param name="keySelector"> key selector function </param>
        /// <returns> Dictionary </returns>
        public static Dictionary<TKey, TSource> ExtensionToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null || keySelector == null) throw new ArgumentNullException();
            Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>();
            foreach (TSource item in source)
            {
                dictionary.Add(keySelector(item), item);
            }
            return dictionary;
        }
        /// <summary>
        /// Groups the elements of a sequence according to a specified key selector function
        /// </summary>
        /// <typeparam name="TSource"> type of the sequence </typeparam>
        /// <typeparam name="TKey"> type of the key </typeparam>
        /// <param name="source"> sequence </param>
        /// <param name="keySelector"> key selector function </param>
        /// <returns> IEnumerable of grouped elements </returns>
        public static IEnumerable<IGrouping<TKey, TSource>> ExtensionGroupBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {

            Dictionary<TKey, List<TSource>> grouping = new Dictionary<TKey, List<TSource>>();
            foreach (TSource item in source)
            {
                if (grouping.ContainsKey(keySelector(item)))
                {
                    grouping[keySelector(item)].Add(item);
                }
                else
                {
                    grouping.Add(keySelector(item), new List<TSource>() { item });
                }
            }
            foreach (var data in grouping)
            {
                yield return new Grouping<TKey, TSource>(data.Key, data.Value);
            }
        }
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource"> type of the sequence </typeparam>
        /// <typeparam name="TKey">type of key </typeparam>
        /// <param name="source"> sequence </param>
        /// <param name="keySelector"> key selector method </param>
        /// <returns> IOrderedEnumerable ordered sequence </returns>
        public static IOrderedEnumerable<TSource> ExtensionOrderByASCENDING<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return new Ordering<TSource, TKey>(source, keySelector,Comparer<TKey>.Default, false);
        }
        public static IOrderedEnumerable<TSource> ExtensionOrderByDESCENDING<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            return new Ordering<TSource, TKey>(source, keySelector, Comparer<TKey>.Default, true);
        }
    }
}
