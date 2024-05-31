using System.Collections.Generic;
using System;

namespace Core
{
    public class SearchResult
    {
        public string[] Query { get; set; }
        public int Hits { get; set; }
        public List<DocumentHit> DocumentHits { get; set; }
        public List<string> Ignored { get; set; }
        public TimeSpan TimeUsed { get; set; }
    }
}
