using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quizer.DataAccess
{
    public static class EnumerableExt
    {
        public static async Task<T[]> ToArrayAsync<T>(this Task<IEnumerable<T>> asyncEnumerable)
        {
            return (await asyncEnumerable).ToArray();
        }
    }
}