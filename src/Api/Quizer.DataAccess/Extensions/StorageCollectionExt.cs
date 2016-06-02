using System.Linq;
using System.Threading.Tasks;
using Quizer.DataAccess.DocumentDb;

namespace Quizer.DataAccess.Extensions
{
    public static class StorageCollectionExt
    {
        public static async Task<IQueryable<T>> QueryableAsync<T>(this Task<IStorageCollection<T>> asyncCollection)
        {
            return (await asyncCollection).Query();
        }
    }
}