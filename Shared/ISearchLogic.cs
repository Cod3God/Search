using System.Threading.Tasks;

namespace Core
{
    public interface ISearchLogic
    {
        //giver et klart sæt af metoder, som enhver klasse, der ønsker at udføre søgninger, skal implementere. 
        //Ved at definere både synkrone og asynkrone versioner af søgemetoderne giver det også fleksibilitet i, 
        //hvordan søgninger kan udføres og integreres i forskellige miljøer eller systemer.
        SearchResult Search(string[] query, int maxAmount);

        Task<SearchResultWithSnippet> SearchAsync(string[] query, int maxAmount);
    }
}
