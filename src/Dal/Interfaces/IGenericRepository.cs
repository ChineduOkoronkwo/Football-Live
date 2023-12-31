using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Interfaces
{
    public interface IGenericRepository<T>
    {
        string CreateSqlCommand { get; }
        string DeleteSqlCommand { get; }
        string UpdateSqlCommand { get; }
        Task<int> Create(T param)
        Task<int> Delete<U>(U? param)
        Task<int> Update(T param)
    }
}