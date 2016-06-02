using System;
using System.Threading.Tasks;

namespace Quizer.DataAccess.DocumentDb
{
    public interface IStorage : IDisposable
    {
        Task<IStorageCollection<T>> CollectionAsync<T>(string id = null);

        IStorage Db(string id = null);
    }
}