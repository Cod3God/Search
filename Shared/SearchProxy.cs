using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Core;
using Shared;
using static System.Net.WebRequestMethods;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;


//Port http://localhost:5034/search


// Loggeroplysninger bliver genereret af HttpClient-objektet


//URL til de forskellige servere
public class SearchProxy : ISearchLogic
{
    private readonly HttpClient _httpClient;
    private readonly string DefaultServerEndPoint = "http://localhost:5036/api/search/";
    private readonly string AlternativeServerEndPoint = "http://localhost:5086/api/search/";
    private readonly string SnippetServiceEndPoint = "http://localhost:5000/api/snippets/";  //Henter fra snippetservice API
    private DateTime lastSearchTime;
    private string lastUsedEndpoint;

    public SearchProxy(HttpClient httpClient)
    {
        _httpClient = httpClient;
        lastSearchTime = DateTime.MinValue;
        lastUsedEndpoint = DefaultServerEndPoint;
    }

    public SearchResult Search(string[] query, int maxAmount)
    {
        throw new NotImplementedException();
    }

    //Modtager array af søgetermer + max
    // kalder de andre metoder for at lave den faktiske søgning og hente
    public async Task<SearchResultWithSnippet> SearchAsync(string[] query, int maxAmount)
    {
        try
        {
            var searchResult = await PerformSearchAsync(query, maxAmount);
            var snippets = await FetchSnippetsAsync(query);

            return new SearchResultWithSnippet
            {
                Result = searchResult,
                Snippets = snippets
            };
        }
        catch (HttpRequestException ex)
        {
            return new SearchResultWithSnippet
            {
                Result = new SearchResult
                {
                    Hits = 0,
                    DocumentHits = new List<DocumentHit>(),
                    Query = query,
                    Ignored = new List<string>(),
                    TimeUsed = TimeSpan.Zero
                },
                Snippets = new List<string>(),
                ErrorMessage = $"Error: {ex.Message}"
            };
        }
    }

    //Modtager søgetermer og bestemmer hvilket endpoint ud fra getcurrentendpoint metode
    private async Task<SearchResult> PerformSearchAsync(string[] query, int maxAmount)
    {
        string currentEndpoint = GetCurrentEndpoint();
        var res = await _httpClient.GetFromJsonAsync<SearchResult>($"{currentEndpoint}{string.Join(",", query)}/{maxAmount}");
        lastSearchTime = DateTime.UtcNow;
        return res;
    }

    //Modtager søgetermerne, henter uddrag fra snippet endpoint
    private async Task<List<string>> FetchSnippetsAsync(string[] query)
    {
        var snippets = await _httpClient.GetFromJsonAsync<List<SnippetResult>>($"{SnippetServiceEndPoint}{query[0]}");
        return snippets.Select(s => s.Snippet).ToList();
    }

    //Bestemmer endpoint ud fra et regelsæt
    private string GetCurrentEndpoint()
    {
        TimeSpan timeSinceLastSearch = DateTime.UtcNow - lastSearchTime;
        if (timeSinceLastSearch.TotalSeconds < 10)
        {
            lastUsedEndpoint = (lastUsedEndpoint == DefaultServerEndPoint) ? AlternativeServerEndPoint : DefaultServerEndPoint;
            return lastUsedEndpoint;
        }

        lastUsedEndpoint = DefaultServerEndPoint;
        return lastUsedEndpoint;
    }
}

//Initiering af snippet / Data format
public class SnippetResult
{
    public string Snippet { get; set; }
}



