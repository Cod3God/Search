
using System;
using Shared;
namespace Core
{
    public class SearchFactory
    {
        public static ISearchLogic GetProxy()
        {
            return new SearchProxy();
        }
    }
}
