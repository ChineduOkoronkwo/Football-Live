using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Interfaces
{
    public interface IGenericReadRepository<T>
    {
        string GetSqlCommand { get; }
        string ListSqlCommand { get; }
        Task<T> Get<U>(U? param);
        Task<IEnumerable<T>> List<U>(U? param);
    }
}