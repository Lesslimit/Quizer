using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Quizer.DataAccess.Contracts;

namespace Quizer.DataAccess.DocumentDb
{
    public interface IDocumentDbQquery<T> : IQuery<T>
    {
        Task<T> FindAsync(string id);

        IDocumentDbQquery<T> Where(Expression<Func<T, bool>> predicate);
        IDocumentDbQquery<TResult> Select<TResult>(Expression<Func<T, TResult>> selector);

        T FirstOrDefault();
        T FirstOrDefault(Func<T, bool> predicate);
    }
}