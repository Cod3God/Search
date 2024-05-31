using System;
using Shared;
using System.Net.Http;

namespace Core
{
    public static class SearchFactory
    {
        public static ISearchLogic GetProxy(HttpClient httpClient)
        {
            return new SearchProxy(httpClient);
        }
    }
}
