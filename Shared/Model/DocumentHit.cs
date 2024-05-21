using System;
using System.Collections.Generic;
using Shared.Model;


namespace Core.Model
{
    public class DocumentHit
    { 

        public BEDocument Document { get; set; }

        public int NoOfHits { get; set; }

        public List<string> Missing { get; set; }

        public string DocumentId { get; set; }
    }
}



/*
namespace Core.Model
{
    public class DocumentHit
    {
        public string Title { get; set; }
        public string ContentSnippet { get; set; }

        public override string ToString()
        {
            return $"{Title}: {ContentSnippet}";
        }
    }
}

*/
