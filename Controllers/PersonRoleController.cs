using Microsoft.AspNetCore.Mvc;
namespace ToDoApi.Controllers;
using Config;
using Npgsql;
using ToDoApi.CRUD_Operations;

[ApiController]
[Route("[controller]")]

public class PersonRoleController : Controller
{
    NpgsqlConnection connection = Config.GetConnection();
    [HttpGet(Name = "GetPersonRole")]

    public IActionResult Get()
    {
        return new ObjectResult(PersonRole.GetAllPersonRoles(connection));
    }

    [HttpGet("{PersonRoleId}", Name = "GetPersonRoleById")]
    public IActionResult Get(int PersonRoleId)
    {
        return new ObjectResult(PersonRole.GetPersonRoleById(connection, PersonRoleId));
    }

    [HttpPut("SoftDelete/{PersonRoleId}", Name = "SoftDeletePersonRole")]
    public IActionResult Deletes(int PersonRoleId)
    {
        return new ObjectResult(PersonRole.SoftDeletePersonRole(connection, PersonRoleId));
    }

    [HttpDelete("{PersonRoleId}", Name = "DeletePersonRoleById")]
    public IActionResult Delete(int PersonRoleId)
    {
        bool deleteSuccessful = PersonRole.DeletePersonRoleById(connection, PersonRoleId);

        if (deleteSuccessful)
        {
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete person role");
        }
    }
}
