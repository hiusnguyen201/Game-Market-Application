namespace GMA.DAL;

using MySql.Data.MySqlClient;

public class DBHelper {
    private static MySqlConnection connection;
    private static string connectionString = @"server=localhost;userid=root;password=123456;port=3306;database=gmadb";

    public static MySqlConnection GetConnection() 
    {
        if(connection == null) 
        {
            connection = new MySqlConnection(connectionString);
        }
        return connection;
    }

    public static MySqlConnection OpenConnection() 
    {
        if(connection == null)
        {
            GetConnection();
        }
        connection.Open();
        return connection;
    }

    public static void CloseConnection() 
    {
        if(connection != null) 
        {
            connection.Close();
        }
    }

    public static MySqlDataReader ExecuteQuery(string query)
    {
        
        MySqlCommand command = new MySqlCommand(query, connection);
        return command.ExecuteReader();
    }
}