/*
namespace Core

{
    public interface ISearchLogic
    {
        SearchResult Search(string[] query, int maxAmount);
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
