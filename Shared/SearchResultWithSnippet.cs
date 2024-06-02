using System.Collections.Generic;

namespace Core
{
    public class SearchResultWithSnippet
    {
        public SearchResult Result { get; set; }
        public List<string> Snippets { get; set; }
        public string ErrorMessage { get; set; } = "Fejl: Ingen resultater"; // Default error message
    }
}
