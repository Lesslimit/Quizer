using System.Linq;

namespace Quizer.DataAccess.DocumentDb
{
    public interface IStorageCollection<out T>
    {
        IQueryable<T> Query();
    }
}