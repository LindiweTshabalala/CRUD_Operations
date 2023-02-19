using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers;
using Config;
using Npgsql;
using ToDoApi.CRUD_Operations;

[ApiController]
[Route("[controller]")]

public class PatientsMedicalConditionController : Controller
{
    NpgsqlConnection connection = Config.GetConnection();
    [HttpGet(Name = "GetPatientsMedicalConditions")]

    public IActionResult Get()
    {
        return new ObjectResult(PatientsMedicalCondition.GetAllMedicalConditions(connection));
    }

    [HttpGet("{PatientId}", Name = "GetPatientsMedicalConditionById")]
    public IActionResult Get(int PatientId)
    {
        return new ObjectResult(PatientsMedicalCondition.GetPatientMedicalConditionById(connection, PatientId));
    }

    [HttpPost(Name = "AddMedicalCondition")]
    public IActionResult Post(int PersonId, string MedicalCondition)
    {

        return new ObjectResult(PatientsMedicalCondition.AddMedicalCondition(connection, PersonId, MedicalCondition));
    }

    [HttpPut("SoftDelete/{PatientId}", Name = "SoftDeleteMedicalCondition")]
    public IActionResult Deletes(int PatientId)
    {
        return new ObjectResult(PatientsMedicalCondition.SoftDeleteMedicalCondition(connection, PatientId));
    }

    [HttpDelete("{PatientId}", Name = "DeletePatientsMedicalConditionById")]
    public IActionResult Delete(int PatientId)
    {
        bool deleteSuccessful = PatientsMedicalCondition.DeletePatientMedicalConditionById(connection, PatientId);

        if (deleteSuccessful)
        {
            // Return a 204 No Content response to indicate success with no response body
            return NoContent();
        }
        else
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete patient's medical condition");
        }
    }
}
