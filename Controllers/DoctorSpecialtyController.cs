using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers;
using Config;
using Npgsql;
using ToDoApi.CRUD_Operations;

[ApiController]
[Route("[controller]")]

public class DoctorSpecialtyController : Controller
{
    NpgsqlConnection connection = Config.GetConnection();
    [HttpGet(Name = "GetDrSpecialty")]

    public IActionResult Get()
    {
        return new ObjectResult(DoctorSpecialty.GetAllSpecialties(connection));
    }

    [HttpGet("{DrSpecialtyId}", Name = "GetDrSpecialtyId")]
    public IActionResult Get(int DrSpecialtyId)
    {
        return new ObjectResult(DoctorSpecialty.GetDrSpecialtyById(connection, DrSpecialtyId));
    }

    [HttpPost(Name = "AddDrSpecialty")]
    public IActionResult Post([Bind("SpecialtyName")] string SpecialtyName)
    {

        return new ObjectResult(DoctorSpecialty.AddDrSpecialty(connection, SpecialtyName));
    }

    [HttpPut("SoftDelete/{DrSpecialtyId}", Name = "SoftDeleteDrSpecialty")]
    public IActionResult Deletes(int DrSpecialtyId)
    {
        return new ObjectResult(DoctorSpecialty.SoftDeleteDrSpecialty(connection, DrSpecialtyId));
    }

    [HttpDelete("{DrSpecialtyId}", Name = "DeleteDrSpecialtyById")]
    public IActionResult Delete(int DrSpecialtyId)
    {
        bool deleteSuccessful = DoctorSpecialty.DeleteDrSpecialtyById(connection, DrSpecialtyId);

        if (deleteSuccessful)
        {
            // Return a 204 No Content response to indicate success with no response body
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete doctor specialty");
        }
    }
}
