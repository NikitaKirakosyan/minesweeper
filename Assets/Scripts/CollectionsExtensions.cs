using System.Collections.Generic;
using System.Linq;

namespace NikitaKirakosyan.Minesweeper
{
    public static class CollectionsExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any() || collection.All(item => item == null);
        }
    }
}
