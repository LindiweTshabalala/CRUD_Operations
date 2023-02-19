using Microsoft.AspNetCore.Mvc;
namespace ToDoApi.Controllers;
using Config;
using Npgsql;
using ToDoApi.CRUD_Operations;

[ApiController]
[Route("[controller]")]

public class HospitalBedsController : Controller
{
    NpgsqlConnection connection = Config.GetConnection();

    [HttpGet(Name = "GetBeds")]
    public IActionResult Get()
    {
        return new ObjectResult(HospitalBeds.GetAllBeds(connection));
    }

    [HttpGet("{BedId}", Name = "GetBedById")]
    public IActionResult Get(int BedId)
    {
        return new ObjectResult(HospitalBeds.GetBedById(connection, BedId));
    }

    [HttpPut("SoftDelete/{BedId}", Name = "SoftBed")]
    public IActionResult Deletes(int BedId)
    {
        return new ObjectResult(HospitalBeds.SoftDeleteBed(connection, BedId));
    }

    [HttpDelete("{BedId}", Name = "DeleteBedById")]
    public IActionResult Delete(int BedId)
    {
        bool deleteSuccessful = HospitalBeds.DeleteBedById(connection, BedId);

        if (deleteSuccessful)
        {
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete hospital bed");
        }
    }
}
