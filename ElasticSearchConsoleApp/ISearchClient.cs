using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ElasticSearchConsoleApp
{
    interface ISearchClient<T> where T:class
    {

        bool Add(T item);

        Task<bool> AddAsync(T item);

        long Add(List<T> items);

        Task<int> AddAsync(List<T> items);

        T Get(string id);

        Task<T> GetAsync(string id);

        bool Update(T item);

        Task<bool> UpdateAsync(T item);

        bool Delete(string id);

        Task<bool> DeleteAsync(string id);

        int Delete(List<T> items);

        Task<int> DeleteAsync(List<T> items);

        List<T> Search(string searchText, int from, int size, Expression<Func<T, object>> fieldToSearchFrom);

        Task<List<T>> SearchAsync(string searchText, int from, int size, Expression<Func<T, object>> fieldToSearchFrom);

        List<T> Search(string searchText, int from, int size);

        Task<List<T>> SearchAsync(string searchText, int from, int size);
    }
}
