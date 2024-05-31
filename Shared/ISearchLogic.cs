using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

/*
namespace Core

{
    public interface ISearchLogic
    {
        SearchResult Search(string[] query, int maxAmount);
    }
}

*/


//Gammel der virker
/*
namespace Core
{
    public interface ISearchLogic
    {
        SearchResultWithSnippet Search(string[] query, int maxAmount);
    }
}
*/


//Med asynkron metode


//Fra gammel!
/*
namespace Core
{
    public interface ISearchLogic
    {
        SearchResultWithSnippet Search(string[] query, int maxAmount);
        Task<SearchResultWithSnippet> SearchAsync(string[] query, int maxAmount);
    }
}
*/

namespace Core
{
    public interface ISearchLogic
    {
        SearchResultWithSnippet Search(string[] query, int maxAmount);
    }



}