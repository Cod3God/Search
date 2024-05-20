using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core;

/*
namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchLogic _searchLogic;

        public SearchController(ISearchLogic searchLogic)
        {
            _searchLogic = searchLogic;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string[] query, [FromQuery] int maxAmount)
        {
            try
            {
                var result = _searchLogic.Search(query, maxAmount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
*/


//Endpoints issue?
namespace SearchAPI.Controllers
{
    [ApiController]
    [Route("api/search")] // Explicitly set the route to "api/search"
    public class SearchController : ControllerBase
    {
        private readonly ISearchLogic _searchLogic;

        public SearchController(ISearchLogic searchLogic)
        {
            _searchLogic = searchLogic;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] string[] query, [FromQuery] int maxAmount)
        {
            try
            {
                var result = _searchLogic.Search(query, maxAmount);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

