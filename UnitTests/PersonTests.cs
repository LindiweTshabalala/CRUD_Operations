
using Npgsql;
using NUnit.Framework;
using ToDoApi.CRUD_Operations;

namespace ToDoApi.UnitTests
{
    public class PersonTests
    {
        [TestFixture]
        public class PersonRepositoryTests
        {
            private NpgsqlConnection _connection;

            [SetUp]
            public void SetUp()
            {
                // Set up database connection for testing
                string connectionString = "Server=localhost;Port=5432;Database=mydatabase;User Id=myuser;Password=mypassword;";
                _connection = new NpgsqlConnection(connectionString);
                _connection.Open();
            }

            [TearDown]
            public void TearDown()
            {
                // Clean up database connection after testing
                _connection.Close();
                _connection.Dispose();
            }

            [Test]
            public void GetAllPeople_ReturnsData()
            {

                // Assumes there is at least one record in the People table
                int expectedCount = 1;

                PersonModel[] people = Person.GetAllPeople(_connection);

                Assert.IsNotNull(people);
                Assert.AreEqual(expectedCount, people.Length);
            }

            [Test]
            public void GetAllPeople_HandlesException()
            {
                // Assumes the connection string is incorrect
                string invalidConnectionString = "Server=invalid;Port=5432;Database=mydatabase;User Id=myuser;Password=mypassword;";
                NpgsqlConnection invalidConnection = new NpgsqlConnection(invalidConnectionString);

                PersonModel[] people = Person.GetAllPeople(invalidConnection);

                Assert.IsEmpty(people);
            }
        }
    }
}
