namespace Core
{
    public interface ISearchLogic
    {
       //SearchResult Search(string[] query, int maxAmount);

        //Rettelse:
        SearchResult Search(string[] query, int maxAmount, int contextLength);
    }
}