using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElasticSearchConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ISearchClient<BlogPost> ISearchClient = new SearchClient<BlogPost>();
            List<BlogPost> blogPostList = new List<BlogPost>();

           var blogPost1 = new BlogPost
            {
                Id = "1",
                Title = "1st blog post",
                Body = "This is first blog post!"
            };

            var blogPost2 = new BlogPost
            {
                Id = "2",
                Title = "2nd blog post",
                Body = "This is second blog post!"
            };

            blogPostList.Add(blogPost1);
            blogPostList.Add(blogPost2);

            // delete multiple records into elastic
            var deletestask = ISearchClient.DeleteAsync(blogPostList);
            Task.WaitAny(deletestask);
            int deletedCount = deletestask.Result;

            // add multiple records into elastic
            var addstask = ISearchClient.AddAsync(blogPostList);
            Task.WaitAny(addstask);
            int addedCount = addstask.Result;

            // add new record into elastic
            var addtask = ISearchClient.AddAsync(blogPost1);
            Task.WaitAny(addtask);
            bool isAdded = addtask.Result;

            // search records for mentioned field from elastic in an asynchronous manner
            var searchedTask = ISearchClient.SearchAsync("second", 1, 10, x => x.Title.ToLower());
            Task.WaitAny(searchedTask);
            List<BlogPost> searchedPosts = searchedTask.Result;

            // search records for any field from elastic
            List<BlogPost> anySearchedPosts = ISearchClient.Search("SEC", 1, 10);

            // get record from elastic using id
            var getTask = ISearchClient.GetAsync("1");
            Task.WaitAny(getTask);
            var blog = getTask.Result;

            // update record into elastic
            blog.Title = "updated title";
            var updateTask = ISearchClient.UpdateAsync(blog);
            Task.WaitAny(updateTask);
            bool isUpdated = updateTask.Result;

            // delete record from elastic
            var deleteTask = ISearchClient.DeleteAsync("1");
            Task.WaitAny(deleteTask);
            bool isDeleted = deleteTask.Result;

            Console.ReadLine();
        }
    }
}
