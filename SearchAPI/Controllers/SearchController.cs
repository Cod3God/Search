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
        public SearchController(IConfiguration _config)
        {
            mConfig = _config;
        }

        [HttpGet]
        [Route("{query}/{maxAmount}")]
        public SearchResult Search(string query, int maxAmount)
        {
            var logic = SearchAPI.Logic.SearchFactory.GetSearchLogic();
            return logic.Search(query.Split(","), maxAmount);
            
        }

        [HttpGet]
        [Route("ping")]
        public string? Ping()
        {
            return mConfig.GetValue<string>("Id");

        }


    }
}

