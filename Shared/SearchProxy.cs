using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Core;
using Microsoft.Data.Sqlite;
using Shared;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Net;
using Core.Model;



// Uden Loadbalancer
/*
namespace Core
{
    public class SearchProxy : ISearchLogic
    {
        private string DefaultserverEndPoint = "http://localhost:5036/api/search/";
        private string AlternativeserverEndPoint = "http://localhost:5086/api/search/";

        //Core.csproj --> Target framework, ændre til dotnet 7

        private HttpClient mHttp;

        public SearchProxy()
        {
            mHttp = new System.Net.Http.HttpClient();
        }

        public SearchResult Search(string[] query, int maxAmount)
        {
            var task = mHttp.GetFromJsonAsync<SearchResult>($"{serverEndPoint}{String.Join(",", query)}/{maxAmount}");
            //var resultStr = response.Content.ReadAsStringAsync().Result;
            var res = task.Result;
            //result = JsonSerializer.Deserialize<SearchResult>(resultStr);
            return res;
        }
    }
}
*/

// Med Loadbalancer

//Port http://localhost:5034/search
/*
public class SearchProxy : ISearchLogic
{
    private string DefaultserverEndPoint = "http://localhost:5036/api/search/";
    private string AlternativeserverEndPoint = "http://localhost:5086/api/search/";

    private HttpClient mHttp;
    private DateTime lastSearchTime;
    private string lastUsedEndpoint;

    public SearchProxy()
    {
        mHttp = new HttpClient();
        lastSearchTime = DateTime.MinValue; // Initialize to a default value
        lastUsedEndpoint = DefaultserverEndPoint; // Initialize to default endpoint
    }

    public SearchResult Search(string[] query, int maxAmount)
    {
        string currentEndpoint = GetCurrentEndpoint();

        Console.WriteLine($"Using API endpoint: {currentEndpoint}"); // Print which API endpoint is being used

        var task = mHttp.GetFromJsonAsync<SearchResult>($"{currentEndpoint}{String.Join(",", query)}/{maxAmount}");
        var res = task.Result;

        // Update the last search time
        lastSearchTime = DateTime.UtcNow;

        return res;
    }

    private string GetCurrentEndpoint()
    {
        // Check if the time elapsed since the last search is less than 10 seconds
        TimeSpan timeSinceLastSearch = DateTime.UtcNow - lastSearchTime;

        // If less than 10 seconds have passed since the last search, toggle the endpoint
        if (timeSinceLastSearch.TotalSeconds < 10)
        {
            // Toggle between the two endpoints based on the last used endpoint
            lastUsedEndpoint = (lastUsedEndpoint == DefaultserverEndPoint) ? AlternativeserverEndPoint : DefaultserverEndPoint;
            return lastUsedEndpoint;
        }

        // If more than 10 seconds have passed, use the default endpoint and update last used endpoint
        lastUsedEndpoint = DefaultserverEndPoint;
        return lastUsedEndpoint;
    }

}
*/

// new-TestBranch
/*
public class SearchProxy : ISearchLogic
{
    private string DefaultserverEndPoint = "http://localhost:5036/api/search/";
    private string AlternativeserverEndPoint = "http://localhost:5086/api/search/";
    private string SnippetServiceEndPoint = "http://localhost:5000/api/snippets/";

    private HttpClient mHttp;
    private DateTime lastSearchTime;
    private string lastUsedEndpoint;

    public SearchProxy()
    {
        mHttp = new HttpClient();
        lastSearchTime = DateTime.MinValue;
        lastUsedEndpoint = DefaultserverEndPoint;
        Console.WriteLine("SearchProxy initialized.");
    }

    public SearchResultWithSnippet Search(string[] query, int maxAmount)
    {
        Console.WriteLine($"Starting synchronous search via proxy with query: {string.Join(", ", query)} and maxAmount: {maxAmount}");
        var searchResult = PerformSearch(query, maxAmount);
        var snippets = FetchSnippets(query);

        Console.WriteLine("Synchronous proxy search completed.");
        return new SearchResultWithSnippet
        {
            Result = searchResult,
            Snippets = snippets
        };
    }

    public async Task<SearchResultWithSnippet> SearchAsync(string[] query, int maxAmount)
    {
        Console.WriteLine($"Starting asynchronous search via proxy with query: {string.Join(", ", query)} and maxAmount: {maxAmount}");
        var searchResult = PerformSearch(query, maxAmount);
        var snippets = await FetchSnippetsAsync(query);

        Console.WriteLine("Asynchronous proxy search completed.");
        return new SearchResultWithSnippet
        {
            Result = searchResult,
            Snippets = snippets
        };
    }

    private SearchResult PerformSearch(string[] query, int maxAmount)
    {
        string currentEndpoint = GetCurrentEndpoint();
        Console.WriteLine($"Using API endpoint: {currentEndpoint}");
        var task = mHttp.GetFromJsonAsync<SearchResult>($"{currentEndpoint}?query={string.Join(",", query)}&maxAmount={maxAmount}");
        var res = task.Result;

        lastSearchTime = DateTime.UtcNow;
        Console.WriteLine("Search result received from API.");
        return res;
    }


    private List<string> FetchSnippets(string[] query)
    {
        Console.WriteLine("Fetching snippets synchronously via SnippetService...");
        var task = mHttp.GetFromJsonAsync<List<SnippetResult>>($"{SnippetServiceEndPoint}{query[0]}");
        var snippets = task.Result;

        Console.WriteLine($"Received {snippets.Count} snippets from SnippetService.");
        return snippets.Select(s => s.Snippet).ToList();
    }

    private async Task<List<string>> FetchSnippetsAsync(string[] query)
    {
        Console.WriteLine("Fetching snippets asynchronously via SnippetService...");
        var snippets = await mHttp.GetFromJsonAsync<List<SnippetResult>>($"{SnippetServiceEndPoint}{query[0]}");

        Console.WriteLine($"Received {snippets.Count} snippets from SnippetService.");
        return snippets.Select(s => s.Snippet).ToList();
    }

    private string GetCurrentEndpoint()
    {
        TimeSpan timeSinceLastSearch = DateTime.UtcNow - lastSearchTime;
        if (timeSinceLastSearch.TotalSeconds < 10)
        {
            lastUsedEndpoint = (lastUsedEndpoint == DefaultserverEndPoint) ? AlternativeserverEndPoint : DefaultserverEndPoint;
            return lastUsedEndpoint;
        }

        lastUsedEndpoint = DefaultserverEndPoint;
        return lastUsedEndpoint;
    }
}

*/
public class SearchProxy : ISearchLogic
{
    private string DefaultServerEndPoint = "http://localhost:5036/api/search/";
    private string AlternativeServerEndPoint = "http://localhost:5086/api/search/";
    private string SnippetServiceEndPoint = "http://localhost:5000/api/snippets/";
    private HttpClient mHttp;
    private DateTime lastSearchTime;
    private string lastUsedEndpoint;

    public SearchProxy()
    {
        mHttp = new HttpClient();
        lastSearchTime = DateTime.MinValue; // Initialize to a default value
        lastUsedEndpoint = DefaultServerEndPoint; // Initialize to default endpoint
        Console.WriteLine("SearchProxy initialized.");
    }
    /*
    public SearchResultWithSnippet Search(string[] query, int maxAmount)
    {
        Console.WriteLine($"Starting synchronous search via proxy with query: {string.Join(", ", query)} and maxAmount: {maxAmount}");
        var searchResult = PerformSearch(query, maxAmount);
        var snippets = FetchSnippets(query);

        Console.WriteLine("Synchronous proxy search completed.");
        return new SearchResultWithSnippet
        {
            Result = searchResult,
            Snippets = snippets
        };
    }

    private SearchResult PerformSearch(string[] query, int maxAmount)
    {
        string currentEndpoint = GetCurrentEndpoint();
        Console.WriteLine($"Using API endpoint: {currentEndpoint}");
        var task = mHttp.GetFromJsonAsync<SearchResult>($"{currentEndpoint}{string.Join(",", query)}/{maxAmount}");
        var res = task.Result;

        lastSearchTime = DateTime.UtcNow;
        Console.WriteLine("Search result received from API.");
        return res;
    }
    */
    public SearchResultWithSnippet Search(string[] query, int maxAmount)
    {
        try
        {
            string currentEndpoint = GetCurrentEndpoint();
            string url = $"{currentEndpoint}/api/search?query={string.Join(",", query)}&maxAmount={maxAmount}";
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadFromJsonAsync<SearchResultWithSnippet>().Result;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new SearchResultWithSnippet
                    {
                        Result = new SearchResult { Hits = 0, DocumentHits = new List<DocumentHit>() },
                        Snippets = new List<string> { "Word doesn't exist, try again." }
                    };
                }
                else
                {
                    throw new HttpRequestException($"Response status code does not indicate success: {response.StatusCode}.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }




    private SearchResult PerformSearch(string[] query, int maxAmount)
    {
        string currentEndpoint = GetCurrentEndpoint();
        var response = mHttp.GetFromJsonAsync<SearchResult>($"{currentEndpoint}{string.Join(",", query)}/{maxAmount}").Result;

        return response;
    }

    private List<string> FetchSnippets(string[] query)
    {
        Console.WriteLine("Fetching snippets synchronously via SnippetService...");
        var task = mHttp.GetFromJsonAsync<List<SnippetResult>>($"{SnippetServiceEndPoint}{query[0]}");
        var snippets = task.Result;

        Console.WriteLine($"Received {snippets.Count} snippets from SnippetService.");
        return snippets.Select(s => s.Snippet).ToList();
    }

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







public class SnippetResult
{
    public string Snippet { get; set; }
}

