using Npgsql;
using ToDoApi.Models;

namespace ToDoApi.CRUD_Operations
{
    public class PersonRole
    {
        public static PersonRoleModel[] GetAllPersonRoles(NpgsqlConnection connection)
        {
            List<PersonRoleModel> personRoleModels = new List<PersonRoleModel>();
            PersonRoleModel personRoleModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM PersonRole", connection))
                {

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            personRoleModel = new PersonRoleModel
                            {
                                PersonRoleId = reader.GetInt32(0),
                                PersonId = reader.GetInt32(1),
                                RoleId = reader.GetInt32(2)
                            };
                            personRoleModels.Add(personRoleModel);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"An error occurred while getting all person roles: {ex.Message}");
            }
            connection.Close();
            return personRoleModels.ToArray();
        }

        public static PersonRoleModel GetPersonRoleById(NpgsqlConnection connection, int PersonRoleId)
        {
            PersonRoleModel personRoleModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM PersonRole WHERE PersonRoleId = @PersonRoleId", connection))
                {
                    cmd.Parameters.AddWithValue("PersonRoleId", PersonRoleId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personRoleModel = new PersonRoleModel
                            {
                                PersonRoleId = reader.GetInt32(0),
                                PersonId = reader.GetInt32(1),
                                RoleId = reader.GetInt32(2)
                            };
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"An error occurred while getting person role by ID: {ex.Message}");
            }
            connection.Close();
            return personRoleModel;
        }

        public static int SoftDeletePersonRole(NpgsqlConnection connection, int PersonRoleId)
        {
            int rowsAffected = 0;
            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE PersonRoles set IsDeleted = cast(1 as bit) where PersonRoleId = @PersonRoleId", connection))
                {
                    command.Parameters.AddWithValue("PersonRoleId", PersonRoleId);
                    rowsAffected = command.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"An error occurred while soft deleting person role: {ex.Message}");
                return rowsAffected;
            }
        }

        public static bool DeletePersonRoleById(NpgsqlConnection connection, int PersonRoleId)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM PersonRole WHERE PersonRoleId = @PersonRoleId", connection))
                {
                    cmd.Parameters.AddWithValue("PersonRoleId", PersonRoleId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"An error occurred while deleting person role by ID: {ex.Message}");
                connection.Close();
                return false;
            }
        }
    }
}
