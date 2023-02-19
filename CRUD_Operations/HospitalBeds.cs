using Npgsql;
using ToDoApi.Models;

namespace ToDoApi.CRUD_Operations
{
    public class HospitalBeds
    {
        public static HospitalBedsModel[] GetAllBeds(NpgsqlConnection connection)
        {
            List<HospitalBedsModel> bedsModels = new List<HospitalBedsModel>();
            HospitalBedsModel bedsModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM HospitalBeds", connection))
                {

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bedsModel = new HospitalBedsModel
                            {
                                BedId = reader.GetInt32(0),
                                WardId = reader.GetInt32(1),
                                PersonId = reader.GetInt32(2)
                            };
                            bedsModels.Add(bedsModel);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"An error occurred while getting all beds in the hospital: {ex.Message}");
            }
            connection.Close();
            return bedsModels.ToArray();
        }

        public static HospitalBedsModel GetBedById(NpgsqlConnection connection, int BedId)
        {
            HospitalBedsModel bedModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM HospitalBeds WHERE BedId = @BedId", connection))
                {
                    cmd.Parameters.AddWithValue("BedId", BedId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bedModel = new HospitalBedsModel
                            {
                                BedId = reader.GetInt32(0),
                                WardId = reader.GetInt32(1),
                                PersonId = reader.GetInt32(2)
                            };
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"An error occurred while getting hospital bed by ID: {ex.Message}");
            }
            connection.Close();
            return bedModel;
        }

        public static int SoftDeleteBed(NpgsqlConnection connection, int BedId)
        {
            int rowsAffected = 0;
            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE HospitalBeds set IsDeleted = cast(1 as bit) where BedId = @BedId", connection))
                {
                    command.Parameters.AddWithValue("BedId", BedId);
                    rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"An error occurred while soft deleting hospital bed: {ex.Message}");
                return rowsAffected;
            }
        }

        public static bool DeleteBedById(NpgsqlConnection connection, int BedId)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM HospitalBeds WHERE BedId = @BedId", connection))
                {
                    cmd.Parameters.AddWithValue("BedId", BedId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"An error occurred while deleting hospital bed by ID: {ex.Message}");
                connection.Close();
                return false;
            }
        }
    }
}

