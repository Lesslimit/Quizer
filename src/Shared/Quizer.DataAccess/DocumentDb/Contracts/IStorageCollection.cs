using System.Threading;
using System.Threading.Tasks;
using Quizer.Domain.Contracts;

namespace Quizer.DataAccess.DocumentDb
{
    public interface IStorageCollection<T> where T : IDocument
    {
        IDocumentDbQquery<T> Query();
        Task AddAsync(T document, CancellationToken cancelToken = default(CancellationToken));
        Task UpdateAsync(T document);
        Task DeleteAsync(T document);
    }
}