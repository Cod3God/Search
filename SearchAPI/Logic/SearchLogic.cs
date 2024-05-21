using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Core.Model;

namespace SearchAPI.Logic
{
    public class SearchLogic : ISearchLogic
    {
        IDatabase mDatabase;

        Dictionary<string, int> mWords;

        public SearchLogic(IDatabase database)
        {
            mDatabase = database;
        }

        public SearchResultWithSnippet Search(string[] query, int maxAmount)
        {
            // Perform the search
            var searchResult = PerformSearch(query, maxAmount);

            // Snippets can be fetched from the snippet microservice
            var snippets = FetchSnippets(query);

            return new SearchResultWithSnippet
            {
                Result = searchResult,
                Snippets = snippets
            };
        }

        public async Task<SearchResultWithSnippet> SearchAsync(string[] query, int maxAmount)
        {
            // Perform the search
            var searchResult = PerformSearch(query, maxAmount);

            // Fetch snippets asynchronously from the snippet microservice
            var snippets = await FetchSnippetsAsync(query);

            return new SearchResultWithSnippet
            {
                Result = searchResult,
                Snippets = snippets
            };
        }

        private SearchResult PerformSearch(string[] query, int maxAmount)
        {
            List<string> ignored;

            DateTime start = DateTime.Now;

            // Convert words to wordids
            var wordIds = mDatabase.GetWordIds(query, out ignored);

            // Perform the search - get all docIds
            var docIds = mDatabase.GetDocuments(wordIds);

            // Get ids for the first maxAmount             
            var top = new List<int>();
            foreach (var p in docIds.GetRange(0, Math.Min(maxAmount, docIds.Count)))
                top.Add(p.Key);

            // Compose the result
            // All the documentHit
            List<DocumentHit> docresult = new List<DocumentHit>();
            int idx = 0;
            foreach (var doc in mDatabase.GetDocDetails(top))
            {
                var missing = mDatabase.WordsFromIds(mDatabase.getMissing(doc.mId, wordIds));

                docresult.Add(new DocumentHit
                {
                    Document = doc,
                    NoOfHits = docIds[idx++].Value,
                    Missing = missing
                });
            }

            return new SearchResult
            {
                Query = query,
                Hits = docIds.Count,
                DocumentHits = docresult,
                Ignored = ignored,
                TimeUsed = DateTime.Now - start
            };
        }

        private List<string> FetchSnippets(string[] query)
        {
            // Make synchronous call to snippet microservice
            var httpClient = new HttpClient();
            var response = httpClient.GetFromJsonAsync<List<SnippetResult>>($"http://localhost:5000/api/snippets/{string.Join(",", query)}").Result;

            return response.Select(s => s.Snippet).ToList();
        }

        private async Task<List<string>> FetchSnippetsAsync(string[] query)
        {
            // Make asynchronous call to snippet microservice
            var httpClient = new HttpClient();
            var response = await httpClient.GetFromJsonAsync<List<SnippetResult>>($"http://localhost:5000/api/snippets/{string.Join(",", query)}");

            return response.Select(s => s.Snippet).ToList();
        }
    }
}