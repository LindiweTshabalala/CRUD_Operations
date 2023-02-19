using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers;
using Config;
using Npgsql;
using ToDoApi.CRUD_Operations;

[ApiController]
[Route("[controller]")]

public class DoctorController : Controller
{
    NpgsqlConnection connection = Config.GetConnection();
    [HttpGet(Name = "GetDoctor")]

    public IActionResult Get()
    {
        return new ObjectResult(Doctor.GetAllDoctors(connection));
    }

    [HttpGet("{DoctorId}", Name = "GetDoctorById")]
    public IActionResult Get(int DoctorId)
    {
        return new ObjectResult(Doctor.GetDoctorById(connection, DoctorId));
    }

    [HttpPost(Name = "AddDoctor")]
    public IActionResult Post(int PersonId, int DrSpecialtyId)
    {

        return new ObjectResult(Doctor.AddDoctor(connection, PersonId, DrSpecialtyId));
    }

    [HttpPut("SoftDelete/{DoctorId}", Name = "SoftDeleteDoctor")]
    public IActionResult Deletes(int DoctorId)
    {
        return new ObjectResult(Doctor.SoftDeleteDoctor(connection, DoctorId));
    }

    [HttpDelete("{DoctorId}", Name = "DeleteDoctorById")]
    public IActionResult Delete(int DoctorId)
    {
        bool deleteSuccessful = Doctor.DeleteDoctorById(connection, DoctorId);

        if (deleteSuccessful)
        {
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete doctor");
        }
    }
}
