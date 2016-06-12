using System.Linq;

namespace Quizer.DataAccess.Contracts
{
    public interface IQuery<T> : IOrderedQueryable<T>
    {
    }
}