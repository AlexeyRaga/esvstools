using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStore.VSTools
{
    public static class EnumerableExtensions
    {
        public static T Next<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            return source.SkipWhile(predicate).Skip(1).First();
        }

        public static T Next<T>(this IEnumerable<T> source, T item)
        {
            var comparer = EqualityComparer<T>.Default;
            return source.Next(x => !comparer.Equals(x, item));
        }

        public static T Previous<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            //I don't optimize for memory OR for speed here as the collections are super small in this project
            return source.Reverse().Next(predicate);
        }

        public static T Previous<T>(this IEnumerable<T> source, T item)
        {
            //I don't optimize for memory OR for speed here as the collections are super small in this project
            return source.Reverse().Next(item);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }
    }
}
