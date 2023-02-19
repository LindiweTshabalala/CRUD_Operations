using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers;
using Config;
using Npgsql;
using ToDoApi.CRUD_Operations;

[ApiController]
[Route("[controller]")]

public class WardController : Controller
{
    NpgsqlConnection connection = Config.GetConnection();
    [HttpGet(Name = "GetWard")]

    public IActionResult Get()
    {
        return new ObjectResult(Ward.GetAllWards(connection));
    }

    [HttpGet("{WardId}", Name = "GetWardId")]
    public IActionResult Get(int WardId)
    {
        return new ObjectResult(Ward.GetWardById(connection, WardId));
    }

    [HttpPost(Name = "AddWard")]
    public IActionResult Post(string WardName, int BedCount)
    {

        return new ObjectResult(Ward.AddWard(connection, WardName, BedCount));
    }

    [HttpPut("SoftDelete/{WardId}", Name = "SoftDeleteWard")]
    public IActionResult Deletes(int WardId)
    {
        return new ObjectResult(Ward.SoftDeleteWard(connection, WardId));
    }

    [HttpDelete("{WardId}", Name = "DeleteWardById")]
    public IActionResult Delete(int WardId)
    {
        bool deleteSuccessful = Ward.DeleteWard(connection, WardId);

        if (deleteSuccessful)
        {
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete ward");
        }
    }
}
