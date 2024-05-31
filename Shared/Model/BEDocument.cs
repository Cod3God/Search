using System;

namespace Shared.Model
{
    public class BEDocument
    {
        public int mId { get; set; }
        public string mUrl { get; set; }
        public string mIdxTime { get; set; }
        public string mCreationTime { get; set; }
        public string Title { get; set; } // Added Title
        public string Snippet { get; set; } // Added Snippet
    }
}
