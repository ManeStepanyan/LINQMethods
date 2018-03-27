using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQMethods
{
    public class ClassForWHERE<TSource> : IEnumerable<TSource>
    { /// <summary>
      /// Class for implementation of where method
      /// </summary>
        private readonly IEnumerable<TSource> source;
        private readonly Func<TSource, bool> predicate;
        public ClassForWHERE(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            this.source = source;
            this.predicate = predicate;
        }
        public IEnumerator<TSource> GetEnumerator()
        {
            return new WhereEnumerator<TSource>(this.source, this.predicate);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        /// <summary>
        /// Creating an IEnumerator for iterating through the collection altered by Where method
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        public class WhereEnumerator<TSource> : IEnumerator<TSource>
        {
            private readonly IEnumerable<TSource> source;
            private readonly IEnumerator<TSource> enumerator;
            Func<TSource, bool> predicate;
            public WhereEnumerator(IEnumerable<TSource> Source, Func<TSource, bool> predicate)
            {
                this.enumerator = Source.GetEnumerator();
                this.predicate = predicate;

            }
            public TSource Current { get { return this.enumerator.Current; } }

            object IEnumerator.Current { get { return Current; } }

            public void Dispose()
            {
                this.enumerator.Dispose();
            }

            public bool MoveNext()
            {
                while (this.enumerator.MoveNext())
                {
                    if (this.predicate(this.enumerator.Current))
                    {
                        return true;
                    }
                }
                return false;

            }

            public void Reset()
            {
                this.enumerator.Reset();
            }
        }

    }
}