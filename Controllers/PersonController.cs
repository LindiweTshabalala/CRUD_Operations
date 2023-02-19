using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers;
using Config;
using Microsoft.Extensions.Configuration;
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
    public IActionResult Post(string FirstName, string LastName, DateTime DateOfBirth, bool IsDeleted)
    {

        return new ObjectResult(Person.AddPerson(connection, FirstName, LastName, DateOfBirth, IsDeleted));
    }

    [HttpPut("SoftDelete/{PersonId}", Name = "SoftDeletePersonById")]
    public IActionResult Deletes(int PersonId)
    {
        return new ObjectResult(Person.SoftDeletePersonById(connection, PersonId));
    }

    [HttpPut("{id}")]
    public IActionResult UpdatePerson(int id, [FromBody] PersonModel update)
    {
        //if (!PersonExists(connection, id))
        //{
        //    return NotFound(); // return 404 if person with the given ID doesn't exist
        //}

        if (update == null || (update.FirstName == null && update.LastName == null && update.DateOfBirth == null))
        {
            return BadRequest(); 
        }

        bool result = Person.UpdatePerson(connection, id, update.FirstName, update.LastName, update.DateOfBirth);

        if (result)
        {
            return Ok(); 
        }
        else
        {
            return StatusCode(500); 
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
