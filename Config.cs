using Npgsql;

namespace Config
{
    public class Config
    {
        const string ConfigString = "Server=localhost;Port=5432;User Id=postgres;Password=Lindiwe#94;Database=myDatabase";
        public static NpgsqlConnection connection = new NpgsqlConnection(ConfigString);

        public static NpgsqlConnection GetConnection()
        {
            connection.Open();
            return connection;
        }
    }
}


   
