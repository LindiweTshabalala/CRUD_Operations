using Npgsql;
using ToDoApi.Models;

namespace ToDoApi.CRUD_Operations
{
    public class Admission
    {
        public static AdmissionModel[] GetAllAdmissions(NpgsqlConnection connection)
        {
            List<AdmissionModel> admissionModels = new List<AdmissionModel>();
            AdmissionModel admissionModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Admissions", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            admissionModel = new AdmissionModel
                            {
                                AdmissionId = reader.GetInt32(0),
                                PersonId = reader.GetInt32(1),
                                DateOfAdmission = reader.GetDateTime(2),
                                DateOfDischarge = reader.GetDateTime(3)
                            };
                            admissionModels.Add(admissionModel);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Error retrieving all admissions: {ex.Message}");
            }
            connection.Close();
            return admissionModels.ToArray();
        }

        public static AdmissionModel GetAdmissionById(NpgsqlConnection connection, int AdmissionId)
        {
            AdmissionModel admission = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Admissions WHERE AdmissionId = @AdmissionId", connection))
                {
                    cmd.Parameters.AddWithValue("AdmissionId", AdmissionId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            admission = new AdmissionModel
                            {
                                AdmissionId = reader.GetInt32(0),
                                PersonId = reader.GetInt32(1),
                                DateOfAdmission = reader.GetDateTime(2),
                                DateOfDischarge = reader.GetDateTime(3)
                            };
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Error retrieving admission by id: {ex.Message}");
            }
            connection.Close();
            return admission;
        }

        public static AdmissionModel AddAdmission(NpgsqlConnection connection, int PersonId, DateTime DateOfAdmission, DateTime DateOfDischarge)
        {
            AdmissionModel admission = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO Admissions (PersonId, DateOfAdmission, DateOfDischarge) VALUES (@PersonId, @DateOfAdmission, @DateOfDischarge)", connection))
                {
                    cmd.Parameters.AddWithValue("PersonId", PersonId);
                    cmd.Parameters.AddWithValue("DateOfAdmission", DateOfAdmission);
                    cmd.Parameters.AddWithValue("DateOfDischarge", DateOfDischarge);
                    cmd.ExecuteNonQuery();

                    admission.PersonId = PersonId;
                    admission.DateOfAdmission = DateOfAdmission;
                    admission.DateOfDischarge = DateOfDischarge;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Error adding admission: {ex.Message}");
            }
            connection.Close();
            return admission;
        }

        public static int SoftDeleteAdmission(NpgsqlConnection connection, int AdmissionId)
        {
            int rowsAffected = 0;
            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE Admissions set IsDeleted=cast(1 as bit) where AdmissionId = @Admission", connection))
                {
                    command.Parameters.AddWithValue("AdmissionId", AdmissionId);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error executing SoftDeleteAdmission: " + ex.Message);
            }
            connection.Close();
            return rowsAffected;
        }

        public static bool DeleteAdmissionById(NpgsqlConnection connection, int admissionId)
        {
            int rowsAffected = 0;
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Admissions WHERE AdmissionId = @AdmissionId", connection))
                {
                    cmd.Parameters.AddWithValue("AdmissionId", admissionId);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error executing DeleteAdmissionById: " + ex.Message);
            }
            connection.Close();
            return rowsAffected > 0;
        }
    }
}
