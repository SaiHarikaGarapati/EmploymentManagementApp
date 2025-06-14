
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Employment_Management.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StateController : ControllerBase
{
    private readonly DbHelper _db;

    public StateController(DbHelper db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var dt = _db.ExecuteQuery("SELECT Name FROM State");
        var states = dt.AsEnumerable().Select(r => r.Field<string>("Name")).ToList();
        return Ok(states);
    }
}


