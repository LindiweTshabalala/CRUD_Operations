using Npgsql;
using ToDoApi.Models;

namespace ToDoApi.CRUD_Operations
{
    public class Role
    {
        public static RoleModel[] GetAllRoles(NpgsqlConnection connection)
        {
            List<RoleModel> roleModels = new List<RoleModel>();
            RoleModel roleModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Roles", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roleModel = new RoleModel
                            {
                                RoleId = reader.GetInt32(0),
                                RoleName = reader.GetString(1),
                            };
                            roleModels.Add(roleModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            connection.Close();
            return roleModels.ToArray();
        }

        public static RoleModel GetRoleById(NpgsqlConnection connection, int RoleId)
        {
            RoleModel roleModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM Roles WHERE RoleId = @RoleId", connection))
                {
                    cmd.Parameters.AddWithValue("RoleId", RoleId);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            roleModel = new RoleModel
                            {
                                RoleId = reader.GetInt32(0),
                                RoleName = reader.GetString(1)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            connection.Close();
            return roleModel;
        }

        public static RoleModel AddRole(NpgsqlConnection connection, string RoleName)
        {
            RoleModel role = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO Roles (RoleName) VALUES (@RoleName)", connection))
                {
                    cmd.Parameters.AddWithValue("RoleName", RoleName);
                    cmd.ExecuteNonQuery();
                    role.RoleName = RoleName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            connection.Close();
            return role;
        }

        public static int SoftDeleteRole(NpgsqlConnection connection, int RoleId)
        {
            int rowsAffected = 0;
            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE Roles set IsDeleted = cast(1 as bit) where RoleId = @RoleId", connection))
                {
                    command.Parameters.AddWithValue("RoleId", RoleId);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
            connection.Close();
            return rowsAffected;
        }

        public static bool DeleteRoleById(NpgsqlConnection connection, int RoleId)
        {
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM Roles WHERE RoleId = @RoleId", connection))
                {
                    cmd.Parameters.AddWithValue("RoleId", RoleId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected > 0; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

    }
}
