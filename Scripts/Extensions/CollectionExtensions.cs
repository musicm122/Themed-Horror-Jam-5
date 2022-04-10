using System;
using System.Collections.Generic;
using System.Linq;

namespace ThemedHorrorJam5.Scripts.Extensions
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) => collection?.Any() != true;

        public static void RemoveOne<T>(this List<T> collection, Func<T, bool> match) =>
            collection.Remove(collection.First(match));

        public static void RemoveAmt<T>(this List<T> collection, Func<T, bool> match, int amt)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                while (amt > 0 && collection.Any(match))
                {
                    if (match(collection[i]))
                    {
                        collection.Remove(collection[i]);
                        amt--;
                    }
                }
            }
        }
    }
}