using Npgsql;
using ToDoApi.Models;

namespace ToDoApi.CRUD_Operations
{
    public class Doctor
    {
        public static DoctorModel[] GetAllDoctors(NpgsqlConnection connection)
        {
            List<DoctorModel> doctorModels = new List<DoctorModel>();
            DoctorModel doctorModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Doctors", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            doctorModel = new DoctorModel
                            {
                                DoctorId = reader.GetInt32(0),
                                PersonId = reader.GetInt32(1),
                                DrSpecialtyId = reader.GetInt32(2),
                            };
                            doctorModels.Add(doctorModel);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("An error occurred while retrieving all doctors: " + ex.Message);
            }
            connection.Close();
            return doctorModels.ToArray();
        }

        public static DoctorModel GetDoctorById(NpgsqlConnection connection, int DoctorId)
        {
            DoctorModel doctor = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Doctors WHERE DoctorId = @DoctorId", connection))
                {
                    cmd.Parameters.AddWithValue("DoctorId", DoctorId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            doctor = new DoctorModel
                            {
                                DoctorId = reader.GetInt32(0),
                                PersonId = reader.GetInt32(1),
                                DrSpecialtyId = reader.GetInt32(2),
                            };
                        }
                    }
                }
                connection.Close();
                return doctor;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while getting doctor by ID", ex);
            }
        }

        public static DoctorModel AddDoctor(NpgsqlConnection connection, int PersonId, int DrSpecialtyId)
        {
            DoctorModel doctor = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO Doctors (PersonId, DrSpecialtyId) VALUES (@PersonId, @DrSpecialtyId)", connection))
                {
                    cmd.Parameters.AddWithValue("PersonId", PersonId);
                    cmd.Parameters.AddWithValue("DrSpecialtyId", DrSpecialtyId);

                    cmd.ExecuteNonQuery();
                    doctor.PersonId = PersonId;
                    doctor.DrSpecialtyId = DrSpecialtyId;
                }
                connection.Close();
                return doctor;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding a new doctor", ex);
            }
        }

        public static int SoftDeleteDoctor(NpgsqlConnection connection, int DoctorId)
        {
            int rowsAffected = 0;
            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE Doctors set IsDeleted = cast(1 as bit) where DoctorId = @DoctorId", connection))
                {
                    command.Parameters.AddWithValue("DoctorId", DoctorId);
                    rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while soft deleting doctor", ex);
            }
        }

        public static bool DeleteDoctorById(NpgsqlConnection connection, int DoctorId)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Doctors WHERE DoctorId = @DoctorId", connection))
                {
                    cmd.Parameters.AddWithValue("DoctorId", DoctorId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; 
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting doctor by ID", ex);
            }
        }

    }
}

