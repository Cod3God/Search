using System.Threading.Tasks;

namespace Core
{
    public interface ISearchLogic
    {
        SearchResult Search(string[] query, int maxAmount);
        Task<SearchResultWithSnippet> SearchAsync(string[] query, int maxAmount);
    }
}
