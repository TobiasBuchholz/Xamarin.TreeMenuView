using System;
using System.Collections.Generic;
using System.Linq;

namespace TreeMenuView.Extensions.System.Linq
{
    public static class EnumerableExtension
    {
        public static bool TryFirst<T>(this IEnumerable<T> @this, Func<T, bool> filter, out T result) 
        {
            result = default(T);
            foreach(var item in @this) {
                if(filter(item)) {
                    result = item;
                    return true;
                }
            }
            return false;
        }

        public static IEnumerable<IEnumerable<int>> GroupConsecutive(this IEnumerable<int> src) 
        {
            var more = false; 
            
            IEnumerable<int> ConsecutiveSequence(IEnumerator<int> csi) {
                int prevCurrent;
                do {
                    yield return (prevCurrent = csi.Current);
                } while((more = csi.MoveNext()) && csi.Current-prevCurrent == 1);
            }

            var si = src.GetEnumerator();
            if(si.MoveNext()) {
                do {
                    yield return ConsecutiveSequence(si).ToList();
                }
                while(more);
            }
        }

        public static bool SequenceEqualSafe<T>(this IEnumerable<T> @this, IEnumerable<T> other)
        {
            if(@this == null && other == null) {
                return true;
            } else if(@this == null) {
                return false;
            } else if(other == null) {
                return false;
            } else {
                return @this.SequenceEqual(other);
            }
        }

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> @this)
        {
            return @this.Where(x => x != null);
        }

        public static IEnumerable<bool> WhereIsTrue(this IEnumerable<bool> @this)
        {
            return @this.Where(x => x);
        }
        
        public static IEnumerable<bool> WhereIsFalse(this IEnumerable<bool> @this)
        {
            return @this.Where(x => !x);
        }
        
        public static bool TryRemoveFirst<T>(this IList<T> @this, Func<T, bool> filter)
        {
            if(@this.TryFirst(filter, out var result)) {
                @this.Remove(result);
                return true;
            }
            return false;
        }
        
        public static IEnumerable<int> Increment(this IEnumerable<int> @this, int byValue = 1)
        {
            return @this.Select(x => x + byValue);
        }
        
        public static IEnumerable<int> Decrement(this IEnumerable<int> @this, int byValue = 1)
        {
            return @this.Select(x => x - byValue);
        }
        
		public static T[] ToSingleArray<T>(this T @this)
		{
			var array = new T[1];
			array[0] = @this;
			return array;
		}
    }
}