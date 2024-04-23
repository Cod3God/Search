using System;
using Core;
namespace SearchAPI.Logic
{
    public class SearchFactory
    {

        public static ISearchLogic GetSearchLogic()
        {
            return new SearchLogic(new Database());
        }
    }
}

