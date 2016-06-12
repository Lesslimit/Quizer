using System.Threading.Tasks;
using Quizer.Domain.Contracts;

namespace Quizer.DataAccess.DocumentDb
{
    public interface IStorageDb
    {
        Task CreateIfNotExists(string id);
        Task<IStorageCollection<T>> CollectionAsync<T>(string id = null) where T : IDocument;
    }
}