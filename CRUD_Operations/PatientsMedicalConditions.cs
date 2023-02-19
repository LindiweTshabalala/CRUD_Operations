using Npgsql;
using ToDoApi.Models;

namespace ToDoApi.CRUD_Operations
{
    public class PatientsMedicalCondition
    {
        public static PatientsMedicalConditionModel[] GetAllMedicalConditions(NpgsqlConnection connection)
        {
            List<PatientsMedicalConditionModel> patientMedicalConditionModels = new List<PatientsMedicalConditionModel>();
            PatientsMedicalConditionModel patientMedicalConditionModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM PatientsMedicalCondition", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            patientMedicalConditionModel = new PatientsMedicalConditionModel
                            {
                                PatientId = reader.GetInt32(0),
                                PersonId = reader.GetInt32(1),
                                MedicalCondition = reader.GetString(2)

                            };
                            patientMedicalConditionModels.Add(patientMedicalConditionModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while getting all medical conditions: " + ex.Message);
            }
            connection.Close();
            return patientMedicalConditionModels.ToArray();
        }

        public static PatientsMedicalConditionModel GetPatientMedicalConditionById(NpgsqlConnection connection, int PatientId)
        {
            PatientsMedicalConditionModel patientMedicalCondition = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM PatientsMedicalCondition WHERE PatientId = @PatientId", connection))
                {
                    cmd.Parameters.AddWithValue("PatientId", PatientId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            patientMedicalCondition = new PatientsMedicalConditionModel
                            {
                                PatientId = reader.GetInt32(0),
                                PersonId = reader.GetInt32(1),
                                MedicalCondition = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while getting medical condition by ID: " + ex.Message);
            }
            connection.Close();
            return patientMedicalCondition;
        }

        public static PatientsMedicalConditionModel AddMedicalCondition(NpgsqlConnection connection, int PersonId, string MedicalCondition)
        {
            PatientsMedicalConditionModel medicalCondition = new();

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO PatientsMedicalCondition (PersonId, MedicalCondition) VALUES (@PersonId, @MedicalCondition)", connection))
                {
                    cmd.Parameters.AddWithValue("PersonId", PersonId);
                    cmd.Parameters.AddWithValue("MedicalCondition", MedicalCondition);
                    cmd.ExecuteNonQuery();

                    medicalCondition.PersonId = PersonId;
                    medicalCondition.MedicalCondition = MedicalCondition;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding medical condition: " + ex.Message);
            }
            connection.Close();
            return medicalCondition;
        }

        public static int SoftDeleteMedicalCondition(NpgsqlConnection connection, int PatientId)
        {
            int rowsAffected = 0;

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE PatientsMedicalCondition set IsDeleted = cast(1 as bit) where PatientId = @PatientId", connection))
                {
                    command.Parameters.AddWithValue("PatientId", PatientId);
                    rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while soft deleting medical condition: " + ex.Message);
            }
        }

        public static bool DeletePatientMedicalConditionById(NpgsqlConnection connection, int PatientId)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM PatientsMedicalCondition WHERE PatientId = @PatientId", connection))
                {
                    cmd.Parameters.AddWithValue("PatientId", PatientId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting medical condition: " + ex.Message);
            }
        }

    }
}
