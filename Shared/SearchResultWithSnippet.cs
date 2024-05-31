using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class SearchResultWithSnippet
    {
        public SearchResult Result { get; set; }
        public List<string> Snippets { get; set; }
    }

}