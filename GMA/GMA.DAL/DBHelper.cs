using MySql.Data.MySqlClient;

namespace GMA.DAL;

public class DBHelper {
    private static MySqlConnection connection;
    private static string connectionString = @"server=localhost;port=3306;userid=root;password=Thephe789!;database=GMADB";

    public static MySqlConnection GetConnection() 
    {
        if(connection == null) connection = new MySqlConnection(connectionString);
        return connection;
    }

    public static MySqlConnection OpenConnection() 
    {
        if(connection == null) GetConnection();
        connection.Open();
        return connection;
    }

    public static void CloseConnection() 
    {
        if(connection != null) connection.Close();
    }

    public static MySqlDataReader ExecuteQuery(string query)
    {
        MySqlCommand command = new MySqlCommand(query, connection);
        return command.ExecuteReader();
    }
}