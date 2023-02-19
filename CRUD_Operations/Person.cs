using System;
using System.Collections;
using Npgsql;

namespace ToDoApi.CRUD_Operations
{
    public class Person
    {
        public static PersonModel[] GetAllPeople(NpgsqlConnection connection)
        {
            List<PersonModel> personModels = new List<PersonModel>();
            
            PersonModel personModel = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM People", connection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            personModel = new PersonModel
                            {
                                PersonId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                DateOfBirth = reader.GetDateTime(3),
                                IsDeleted = reader.GetBoolean(4)
                            };
                            personModels.Add(personModel);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error executing GetAllPeople: " + ex.Message);
            }
            connection.Close();
            return personModels.ToArray();
        }

        public static PersonModel GetPersonById(NpgsqlConnection connection, int PersonId)
        {
            PersonModel person = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM People WHERE PersonId = @PersonId", connection))
                {
                    cmd.Parameters.AddWithValue("PersonId", PersonId);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            person = new PersonModel
                            {
                                PersonId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                DateOfBirth = reader.GetDateTime(3),
                                IsDeleted = reader.GetBoolean(4)
                            };
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error executing GetPersonById: " + ex.Message);
            }
            connection.Close();
            return person;
        }

        public static PersonModel AddPerson(NpgsqlConnection connection, string FirstName, string LastName, DateTime DateOfBirth, bool IsDeleted)
        {
            PersonModel person = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO People (FirstName, LastName, DateOfBirth, IsDeleted) VALUES (@FirstName, @LastName, @DateOfBirth, @IsDeleted)", connection))
                {
                    cmd.Parameters.AddWithValue("FirstName", FirstName);
                    cmd.Parameters.AddWithValue("LastName", LastName);
                    cmd.Parameters.AddWithValue("DateOfBirth", DateOfBirth);
                    cmd.Parameters.AddWithValue("IsDeleted", IsDeleted);
                    cmd.ExecuteNonQuery();
                    person.FirstName = FirstName;
                    person.LastName = LastName;
                    person.DateOfBirth = DateOfBirth;
                    person.IsDeleted= IsDeleted;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error executing AddPerson: " + ex.Message);
            }
            connection.Close();
            return person;
        }

        public static int SoftDeletePersonById(NpgsqlConnection connection, int PersonId)
        {
            int rowsAffected = 0;
            try
            {
                using (NpgsqlCommand command = new NpgsqlCommand("UPDATE People set IsDeleted = cast(1 as bit) where PersonId = @PersonId", connection))
                {
                    command.Parameters.AddWithValue("PersonId", PersonId);
                    rowsAffected = command.ExecuteNonQuery();
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error executing SoftDeletePersonById: " + ex.Message);
            }
            connection.Close();
            return rowsAffected;
        }

        public static bool DeletePersonById(NpgsqlConnection connection, int personId)
        {
            int rowsAffected = 0;
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM People WHERE PersonId = @PersonId", connection))
                {
                    cmd.Parameters.AddWithValue("PersonId", personId);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error executing DeletePersonById: " + ex.Message);
            }
            connection.Close();
            return rowsAffected > 0;
        }

        public static bool UpdatePerson(NpgsqlConnection connection, int PersonID, string FirstName = null, string LastName = null, DateTime? DateOfBirth = null)
        {
            if (FirstName == null && LastName == null && DateOfBirth == null)
            {
                return false; // no fields to update
            }

            string query = "UPDATE People SET ";
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>();

            if (FirstName != null)
            {
                query += "FirstName = @FirstName, ";
                parameters.Add(new NpgsqlParameter("FirstName", FirstName));
            }

            if (LastName != null)
            {
                query += "LastName = @LastName, ";
                parameters.Add(new NpgsqlParameter("LastName", LastName));
            }

            if (DateOfBirth != null)
            {
                query += "DateOfBirth = @DateOfBirth, ";
                parameters.Add(new NpgsqlParameter("DateOfBirth", DateOfBirth));
            }
            else
            {
                query += "DateOfBirth = DateOfBirth, "; // this line ensures that the value remains unchanged
            }

            // remove the last comma and space
            query = query.Substring(0, query.Length - 2);

            query += " WHERE PersonId = @PersonId";
            parameters.Add(new NpgsqlParameter("PersonId", PersonID));

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                command.Parameters.AddRange(parameters.ToArray());
                int affectedRows = command.ExecuteNonQuery();
                connection.Close();
                return affectedRows > 0;
            }

        }
    }
}

               
