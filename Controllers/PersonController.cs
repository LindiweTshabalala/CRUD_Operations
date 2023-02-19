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
        try
        {
            PersonModel result = Person.GetPersonById(connection, PersonId);
            if (result != null) {
                return new ObjectResult(result);
            }
            return new ObjectResult($"Could not find person with id {PersonId}");
        }
        catch (Exception ex)
        {
           Console.WriteLine (ex);
            return BadRequest("Error returning person by id");
        }
    }

    [HttpPost(Name = "AddPerson")]
    public IActionResult Post(string FirstName, string LastName, DateTime DateOfBirth)
    {

        return new ObjectResult(Person.AddPerson(connection, FirstName, LastName, DateOfBirth));
    }

    [HttpPut("SoftDelete/{PersonId}", Name = "SoftDeletePersonById")]
    public IActionResult Deletes(int PersonId)
    {
        return new ObjectResult(Person.SoftDeletePersonById(connection, PersonId));
    }

    [HttpPut("Update", Name = "UpdatePerson")]
    public IActionResult UpdatePerson(int PersonId, string FirstName, string LastName, DateTime DateOfBirth)
    {
        var updated = Person.UpdatePerson(connection, PersonId, FirstName, LastName, DateOfBirth); 
        if (updated)
        {
            return Ok($"Person with ID {PersonId} successfully updated.");
        }
        else
        {
            return BadRequest($"Failed to update tenant with ID {PersonId}.");
        }
    }

    [HttpDelete("{PersonId}", Name = "DeletePersonById")]
    public IActionResult Delete(int PersonId)
    {
        bool deleteSuccessful = Person.DeletePersonById(connection, PersonId);

        if (deleteSuccessful)
        {          
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete person");
        }
    }
}
