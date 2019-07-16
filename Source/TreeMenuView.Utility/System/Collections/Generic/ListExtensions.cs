namespace System.Collections.Generic
{
    public static class ListExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<int, T> action)
        {
            var i = 0;
            foreach (var item in sequence) {
                action(i, item);
                i++;
            }
        }
    }
}