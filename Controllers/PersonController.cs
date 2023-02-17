using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers;
using Config;
using Npgsql;
using ToDoApi.CRUD_Operations;

[ApiController]
[Route("[controller]")]

public class PersonController : Controller
{
    NpgsqlConnection connection = Config.GetConnection();

    [HttpGet(Name = "GetPeople")]
    public IActionResult Get()
    {
        return new ObjectResult(Person.GetAllPeople(connection));
    }

    [HttpGet("{PersonId}", Name = "GetPersonById")]
    public IActionResult Get(int PersonId)
    {
        return new ObjectResult(Person.GetPersonById(connection, PersonId));
    }

    [HttpPost(Name = "AddPerson")]
    public IActionResult Post([Bind("FirstName")] string FirstName, [Bind("LastName")] string LastName, [Bind("DateOfBirth")] DateTime DateOfBirth)
    {

        return new ObjectResult(Person.AddPerson(connection, FirstName, LastName, DateOfBirth));
    }

    [HttpPut("SoftDelete/{PersonId}", Name = "SoftDeletePersonById")]
    public IActionResult Deletes(int PersonId)
    {
        return new ObjectResult(Person.SoftDeletePersonById(connection, PersonId));
    }

    [HttpDelete("{PersonId}", Name = "DeletePersonById")]
    public IActionResult Delete(int PersonId)
    {
        bool deleteSuccessful = Person.DeletePersonById(connection, PersonId);

        if (deleteSuccessful)
        {
            // Return a 204 No Content response to indicate success with no response body
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete person");
        }
    }
}
