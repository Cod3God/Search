using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core;


<<<<<<< Updated upstream
[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
=======
[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ISearchLogic _searchLogic;

    public SearchController(ISearchLogic searchLogic)
    {
        _searchLogic = searchLogic;
    }

    [HttpGet("{query}/{maxAmount}")]
    public ActionResult<SearchResultWithSnippet> Get([FromRoute] string query, [FromRoute] int maxAmount)
    {
        var result = _searchLogic.Search(new string[] { query }, maxAmount);
        return Ok(result);
    }
}


/*
//Endpoints issue?
namespace SearchAPI.Controllers
>>>>>>> Stashed changes
{
    private readonly ISearchLogic _searchLogic;

    public SearchController(ISearchLogic searchLogic)
    {
        _searchLogic = searchLogic;
    }
<<<<<<< Updated upstream

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






=======

}
*/

>>>>>>> Stashed changes

