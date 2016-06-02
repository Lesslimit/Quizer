using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quizer.DataAccess.Extensions
{
    public static class QueryableExt
    {
        public static async Task<IEnumerable<T>> AsEnumerableAsync<T>(this Task<IQueryable<T>> queryableAsync)
        {
            return (await queryableAsync).AsEnumerable();
        }
    }
}