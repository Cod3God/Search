using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace SearchAPI.Logic
{
    public class SearchLogic : ISearchLogic
    {
        private readonly IDatabase mDatabase;

        public SearchLogic(IDatabase database)
        {
            mDatabase = database;
        }

        public SearchResult Search(string[] query, int maxAmount)
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

        public async Task<SearchResultWithSnippet> SearchAsync(string[] query, int maxAmount)
        {
            return await Task.Run(() =>
            {
                var searchResult = Search(query, maxAmount);
                var snippets = searchResult.DocumentHits.Select(d => "Snippet for " + d.Document.mUrl).ToList();

                return new SearchResultWithSnippet
                {
                    Result = searchResult,
                    Snippets = snippets
                };
            });
        }
    }
}



