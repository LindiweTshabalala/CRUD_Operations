using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers;
using Config;
using Npgsql;
using ToDoApi.CRUD_Operations;

[ApiController]
[Route("[controller]")]

public class AdmissionController : Controller
{
    NpgsqlConnection connection = Config.GetConnection();
    [HttpGet(Name = "GetAdmissions")]

    public IActionResult Get()
    {
        return new ObjectResult(Admission.GetAllAdmissions(connection));
    }

    [HttpGet("{AdmissionId}", Name = "GetAdmissionById")]
    public IActionResult Get(int AdmissionId)
    {
        return new ObjectResult(Admission.GetAdmissionById(connection, AdmissionId));
    }

    [HttpPost(Name = "AddAdmission")]
    public IActionResult Post([Bind("PersonId")] int PersonId, [Bind("DateOfAdmission")] DateTime DateOfAdmission, [Bind("DateOfDischarge")] DateTime DateOfDischarge)
    {

        return new ObjectResult(Admission.AddAdmission(connection, PersonId, DateOfAdmission, DateOfDischarge));
    }

    [HttpPut("SoftDelete/{AdmissionId}", Name = "SoftDeleteAdmission")]
    public IActionResult Deletes(int AdmissionId)
    {
        return new ObjectResult(Admission.SoftDeleteAdmission(connection, AdmissionId));
    }

    [HttpDelete("{AdmissionId}", Name = "DeleteAdmissionById")]
    public IActionResult Delete(int AdmissionId)
    {
        bool deleteSuccessful = Admission.DeleteAdmissionById(connection, AdmissionId);

        if (deleteSuccessful)
        {
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete admission");
        }
    }
}
