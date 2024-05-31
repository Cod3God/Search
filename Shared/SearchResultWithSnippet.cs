using System.Collections.Generic;

namespace Core
{
    public class SearchResultWithSnippet
    {
        public SearchResult Result { get; set; }
        public List<string> Snippets { get; set; }
    }
}
