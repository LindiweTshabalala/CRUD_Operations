using Npgsql;
using ToDoApi.Models;

namespace ToDoApi.CRUD_Operations
{
    public class Ward
    {
        public static WardModel[] GetAllWards(NpgsqlConnection connection)
        {
            List<WardModel> wardModels = new List<WardModel>();
            WardModel wardModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Wards", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            wardModel = new WardModel
                            {
                                WardId = reader.GetInt32(0),
                                WardName = reader.GetString(1),
                                BedCount = reader.GetInt32(2),
                            };
                            wardModels.Add(wardModel);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error retrieving wards: " + ex.Message);
            }

            return wardModels.ToArray();
        }

        public static WardModel GetWardById(NpgsqlConnection connection, int WardId)
        {
            WardModel ward = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Wards WHERE WardId = @WardId", connection))
                {
                    cmd.Parameters.AddWithValue("WardId", WardId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ward = new WardModel
                            {
                                WardId = reader.GetInt32(0),
                                WardName = reader.GetString(1),
                                BedCount = reader.GetInt32(2)
                            };
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error retrieving ward: " + ex.Message);
            }

            return ward;
        }

        public static WardModel AddWard(NpgsqlConnection connection, string WardName, int BedCount)
        {
            WardModel ward = new();

            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO Wards (WardName, BedCount) VALUES (@WardName, @BedCount)", connection))
                {
                    cmd.Parameters.AddWithValue("WardName", WardName);
                    cmd.Parameters.AddWithValue("BedCount", BedCount);

                    cmd.ExecuteNonQuery();
                    ward.WardName = WardName;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error adding ward: " + ex.Message);
            }

            return ward;
        }

        public static int SoftDeleteWard(NpgsqlConnection connection, int WardId)
        {
            int rowsAffected = 0;

            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE Wards set IsDeleted = cast(1 as bit) where WardId = @WardId", connection))
                {
                    command.Parameters.AddWithValue("WardId", WardId);
                    rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error deleting ward: " + ex.Message);
            }

            return rowsAffected;
        }

        public static bool DeleteWard(NpgsqlConnection connection, int WardId)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Wards WHERE WardId = @WardId", connection))
                {
                    cmd.Parameters.AddWithValue("WardId", WardId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error deleting ward: " + ex.Message);
                return false;
            }
        }

    }
}
