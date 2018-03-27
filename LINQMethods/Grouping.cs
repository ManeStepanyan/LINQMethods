using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQMethods
{
    public class Grouping<TKey, TSource> : IGrouping<TKey, TSource>
    {
        private TKey key;
        public IEnumerable<TSource> source;
        public Grouping(TKey key, IEnumerable<TSource> source)
        {
            this.key = key;
            this.source = source;
        }

        public TKey Key { get { return key; } }

        public IEnumerator<TSource> GetEnumerator()
        {
            return source.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }



}
