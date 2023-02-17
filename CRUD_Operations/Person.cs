using System;
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
                                DateOfBirth = reader.GetDateTime(3)
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
                                DateOfBirth = reader.GetDateTime(3)
                            };
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error executing GetPersonById: " + ex.Message);
            }
            return person;
        }

        public static PersonModel AddPerson(NpgsqlConnection connection, string FirstName, string LastName, DateTime DateOfBirth)
        {
            PersonModel person = new();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO People (FirstName, LastName, DateOfBirth) VALUES (@FirstName, @LastName, @DateOfBirth)", connection))
                {
                    cmd.Parameters.AddWithValue("FirstName", FirstName);
                    cmd.Parameters.AddWithValue("LastName", LastName);
                    cmd.Parameters.AddWithValue("DateOfBirth", DateOfBirth);
                    cmd.ExecuteNonQuery();
                    person.FirstName = FirstName;
                    person.LastName = LastName;
                    person.DateOfBirth = DateOfBirth;
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Error executing AddPerson: " + ex.Message);
            }
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
            return rowsAffected > 0;
        }
    }
}

               
