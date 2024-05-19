using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core;

namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private IConfiguration mConfig;
        private readonly ISearchLogic _searchLogic;

        public SearchController(IConfiguration config, ISearchLogic searchLogic)
        {
            mConfig = config;
            _searchLogic = searchLogic;
        }

        [HttpGet]
        [Route("{query}/{maxAmount}")]
        public async Task<SearchResultWithSnippet> Search(string query, int maxAmount)
        {
            Console.WriteLine($"Received search request for query: {query} with maxAmount: {maxAmount}");
            var searchResult = await _searchLogic.SearchAsync(query.Split(","), maxAmount);
            Console.WriteLine("Search request processed successfully.");
            return searchResult;
        }

        [HttpGet]
        [Route("ping")]
        public string? Ping()
        {
            return mConfig.GetValue<string>("Id");
        }
    }
}
