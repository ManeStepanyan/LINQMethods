using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQMethods
{
    public class ClassForSELECT<TSource, TResult> : IEnumerable<TResult>
    {/// <summary>
     /// Class for implementation of Select method
     /// </summary>
        private readonly IEnumerable<TSource> source;
        private readonly Func<TSource, TResult> selector;
        public ClassForSELECT(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            this.source = source;
            this.selector = selector;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            return new SelectEnumerator<TSource, TResult>(source, selector);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
    /// <summary>
    ///IEnumerator for iterating through the collection altered by Select
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class SelectEnumerator<TSource, TResult> : IEnumerator<TResult>
    {
        private readonly IEnumerator<TSource> enumerator;
        private readonly Func<TSource, TResult> selector;
        public SelectEnumerator(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            this.enumerator = source.GetEnumerator();
            this.selector = selector;
        }
        public TResult Current { get { return this.selector(this.enumerator.Current); } }

        object IEnumerator.Current { get { return Current; } }

        public void Dispose()
        {
            enumerator.Dispose();
        }

        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }

        public void Reset()
        {
            enumerator.Reset();
        }
    }
}
