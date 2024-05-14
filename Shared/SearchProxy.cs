using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using Core;
using Shared;
using Shared.Model;
using static System.Net.WebRequestMethods;


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

    //Rigtig kode!
    /*
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
    */
    //Rigtigkode slut!

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


    //Test ny kode

    public SearchResult Search(string[] query, int maxAmount, int contextLength)
    {
        string currentEndpoint = GetCurrentEndpoint();

        Console.WriteLine($"Using API endpoint: {currentEndpoint}"); // Print which API endpoint is being used

        var task = mHttp.GetFromJsonAsync<SearchResult>($"{currentEndpoint}{String.Join(",", query)}/{maxAmount}");

        var res = task.Result;

        // Update the DocumentHits with context
        foreach (var documentHit in res.DocumentHits)
        {
            // Retrieve the text for the document
            string text = GetDocumentText(documentHit.Document, query, contextLength);

            // Set the context for the document hit
            documentHit.Context = text;
        }


        // Update the last search time
        lastSearchTime = DateTime.UtcNow;

        return res;
    }

    public string GetDocumentText(BEDocument document, string[] query, int contextLength)
    {
        // Retrieve the text content of the document using the mUrl property
        string text = RetrieveTextFromUrl(document.mUrl);

        // Calculate the context around the search word
        string context = GetContext(text, query[0], contextLength); // Pass contextLength to GetContext

        return context;
    }


    public string GetContext(string text, string searchWord, int contextLength)
    {
        // Split the text into words
        string[] words = text.Split(" ");

        // Find the index of the search word
        int index = Array.IndexOf(words, searchWord);

        // If the search word is not found, return empty string
        if (index == -1)
            return "";

        // Calculate the start and end index for the context
        int startIndex = Math.Max(index - contextLength, 0);
        int endIndex = Math.Min(index + contextLength, words.Length - 1);

        // Extract the context words
        string[] contextWords = words[startIndex..(endIndex + 1)];

        // Join the context words into a string
        string context = string.Join(" ", contextWords);

        return context;
    }

    private string RetrieveTextFromUrl(string url)
    {
        // Your logic to make an HTTP request to retrieve the text content from the document URL
        // For example, you might use HttpClient to make the request.
        // Replace this with your actual logic to retrieve the text content.

        // For the sake of example, let's say you make an HTTP request to the document URL and retrieve the text content.
        // Here, we'll just return a placeholder text.
        string text = ""; // Placeholder for text content

        // Example logic to make an HTTP request to retrieve the text content
        using (var httpClient = new HttpClient())
        {
            var response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                text = response.Content.ReadAsStringAsync().Result;
            }
        }

        return text;
    }

}