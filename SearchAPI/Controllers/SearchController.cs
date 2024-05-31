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
        private readonly ISearchLogic _searchLogic;

        public SearchController(ISearchLogic searchLogic)
        {
            _searchLogic = searchLogic;
        }

        [HttpGet]
        [Route("{query}/{maxAmount}")]
        public async Task<SearchResultWithSnippet> SearchAsync(string query, int maxAmount)
        {
            return await _searchLogic.SearchAsync(query.Split(","), maxAmount);
        }

        [HttpGet]
        [Route("ping")]
        public string? Ping()
        {
            return "Ping successful";
        }
    }
}


