using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Nest;

namespace ElasticSearchConsoleApp
{
    public class SearchClient<T> : ISearchClient<T> where T : class
    {
        private static ElasticClient elastic;
        private static string defaultIndexName = "myblogindex"; //ConfigurationManager.AppSettings["ElasticDefaultIndexName"];
        private static string endPointUrl = "http://localhost:9200"; //ConfigurationManager.AppSettings["ElasticEndPointUrl"];

        public SearchClient()
        {
            if (elastic == null)
                OpenConnection();
        }

        // open elastic connection
        private void OpenConnection()
        {
            var url = new Uri(endPointUrl);
            var settings = new ConnectionSettings(url).DefaultIndex(defaultIndexName);
            elastic = new ElasticClient(settings);
        }

        // add new record into elastic
        public bool Add(T item)
        {
            try
            {
                return elastic.Index(item).Created;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // add new record into elastic in an asynchronous manner
        public async Task<bool> AddAsync(T item)
        {
            try
            {
                var response = await elastic.IndexAsync(item);
                return response.Created;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // add multiple records into elastic
        public long Add(List<T> items)
        {
            try
            {
                return elastic.IndexMany(items).Took;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // add multiple records into elastic in an asynchronous manner
        public async Task<int> AddAsync(List<T> items)
        {
            try
            {
                var response = await elastic.IndexManyAsync(items);
                return response.Items.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get record from elastic using id
        public T Get(string id)
        {
            try
            {
                return elastic.Get<T>(id).Source;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // get record from elastic using id in an asynchronous manner
        public async Task<T> GetAsync(string id)
        {
            try
            {
                var response = await elastic.GetAsync<T>(id);
                return response.Source;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // update record into elastic
        public bool Update(T item)
        {
            try
            {
                var response =  elastic.Update(new DocumentPath<T>(item), u => u.Doc(item));

                if (response.Version > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // update record into elastic in an asynchronous manner
        public async Task<bool> UpdateAsync(T item)
        {
            try
            {
                var response = await elastic.UpdateAsync(new DocumentPath<T>(item), u => u.Doc(item));

                if (response.Version > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // delete record from elastic
        public bool Delete(string id)
        {
            try
            {
                return elastic.Delete(new DeleteRequest<T>(id)).Found;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // delete record from elastic in an asynchronous manner
        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var response = await elastic.DeleteAsync(new DeleteRequest<T>(id));
                return response.Found;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // delete multiple records from elastic
        public int Delete(List<T> items)
        {
            try
            {
                return elastic.DeleteMany(items).Items.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // delete multiple records from elastic in an asynchronous manner
        public async Task<int> DeleteAsync(List<T> items)
        {
            try
            {
                var response = await elastic.DeleteManyAsync(items);
                return response.Items.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // search records for mentioned field from elastic
        public List<T> Search(string searchText, int from, int size, Expression<Func<T, object>> fieldToSearchFrom)
        {
            try
            {
                var response = elastic.Search<T>(s => s
                                         .From(from - 1) // default index = 0
                                         .Size(size)
                                         .Query(q => q.Match(mq => mq.Field(fieldToSearchFrom).Query(searchText.ToLower())))
                                         );

                return response.Documents.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // search records for mentioned field from elastic in an asynchronous manner
        public async Task<List<T>> SearchAsync(string searchText, int from, int size, Expression<Func<T, object>> fieldToSearchFrom)
        {
            try
            {
                var response = await elastic.SearchAsync<T>(s => s
                                         .From(from - 1) // default index = 0
                                         .Size(size)
                                         .Query(q => q.Match(mq => mq.Field(fieldToSearchFrom).Query(searchText.ToLower())))
                                         );

                return response.Documents.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // search records for any field from elastic
        public List<T> Search(string searchText, int from, int size)
        {
            try
            {
                var response = elastic.Search<T>(s => s
                                      .From(from - 1) // default index = 0
                                      .Take(size)
                                      .Query(qry => qry.Bool(b => b.Must(m => m.QueryString(qs => qs.DefaultField("_all").Query(searchText)))))
                                       );

                return response.Documents.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // search records for any field from elastic in an asynchronous manner
        public async Task<List<T>> SearchAsync(string searchText, int from, int size)
        {
            try
            {
                var response = await elastic.SearchAsync<T>(s => s
                                           .From(from - 1) // default index = 0
                                           .Take(size)
                                           .Query(qry => qry.Bool(b => b.Must(m => m.QueryString(qs => qs.DefaultField("_all").Query(searchText)))))
                                           );

                return response.Documents.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
