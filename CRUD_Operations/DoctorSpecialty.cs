using Npgsql;
using ToDoApi.Models;

namespace ToDoApi.CRUD_Operations
{
    public class DoctorSpecialty
    {
        public static DoctorSpecialtyModel[] GetAllSpecialties(NpgsqlConnection connection)
        {
            List<DoctorSpecialtyModel> drSpecialtyModels = new List<DoctorSpecialtyModel>();
            DoctorSpecialtyModel drSpecialtyModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM DoctorSpecialty", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            drSpecialtyModel = new DoctorSpecialtyModel
                            {
                                DrSpecialtyId = reader.GetInt32(0),
                                SpecialtyName = reader.GetString(1),
                            };
                            drSpecialtyModels.Add(drSpecialtyModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving DoctorSpecialty data: " + ex.Message);
            }

            return drSpecialtyModels.ToArray();
        }

        public static DoctorSpecialtyModel GetDrSpecialtyById(NpgsqlConnection connection, int DrSpecialtyId)
        {
            DoctorSpecialtyModel drSpecialty = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM DoctorSpecialty WHERE DrSpecialtyId = @DrSpecialtyId", connection))
                {
                    cmd.Parameters.AddWithValue("DrSpecialtyId", DrSpecialtyId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            drSpecialty = new DoctorSpecialtyModel
                            {
                                DrSpecialtyId = reader.GetInt32(0),
                                SpecialtyName = reader.GetString(1),
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while retrieving DoctorSpecialty data: " + ex.Message);
            }

            return drSpecialty;
        }

        public static DoctorSpecialtyModel AddDrSpecialty(NpgsqlConnection connection, string SpecialtyName)
        {
            DoctorSpecialtyModel drSpecialty = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO DoctorSpecialty (SpecialtyName) VALUES (@SpecialtyName)", connection))
                {
                    cmd.Parameters.AddWithValue("SpecialtyName", SpecialtyName);

                    cmd.ExecuteNonQuery();
                    drSpecialty.SpecialtyName = SpecialtyName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return drSpecialty;
        }

        public static int SoftDeleteDrSpecialty(NpgsqlConnection connection, int DrSpecialtyId)
        {
            int rowsAffected = 0;
            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE DoctorSpecialty set IsDeleted = cast(1 as bit) where DrSpecialtyId = @DrSpecialtyId", connection))
                {
                    command.Parameters.AddWithValue("DrSpecialtyId", DrSpecialtyId);
                    rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return rowsAffected;
        }

        public static bool DeleteDrSpecialtyById(NpgsqlConnection connection, int DrSpecialtyId)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM DoctorSpecialty WHERE DrSpecialtyId = @DrSpecialtyId", connection))
                {
                    cmd.Parameters.AddWithValue("DrSpecialtyId", DrSpecialtyId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0; 

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting doctor specialty by ID", ex);
            }
        }

    }
}