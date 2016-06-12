using System;

namespace Quizer.DataAccess.DocumentDb
{
    public interface IStorage : IDisposable
    {
        IStorageDb Db(string id = null);
    }
}