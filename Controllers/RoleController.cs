using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers;
using Config;
using Npgsql;
using ToDoApi.CRUD_Operations;

[ApiController]
[Route("[controller]")]

public class RoleController : Controller
{
    NpgsqlConnection connection = Config.GetConnection();
    [HttpGet(Name = "GetRole")]

    public IActionResult Get()
    {
        return new ObjectResult(Role.GetAllRoles(connection));
    }

    [HttpGet("{RoleId}", Name = "GetRoleId")]
    public IActionResult Get(int RoleId)
    {
        return new ObjectResult(Role.GetRoleById(connection, RoleId));
    }

    [HttpPost(Name = "AddRole")]
    public IActionResult Post([Bind("RoleName")] string RoleName)
    {

        return new ObjectResult(Role.AddRole(connection, RoleName));
    }

    [HttpPut("SoftDelete/{RoleId}", Name = "SoftDeleteRole")]
    public IActionResult Deletes(int RoleId)
    {
        return new ObjectResult(Role.SoftDeleteRole(connection, RoleId));
    }

    [HttpDelete("{RoleId}", Name = "DeleteRoleById")]
    public IActionResult Delete(int RoleId)
    {
        bool deleteSuccessful = Role.DeleteRoleById(connection, RoleId);

        if (deleteSuccessful)
        {
            // Return a 204 No Content response to indicate success with no response body
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete role");
        }
    }
}
