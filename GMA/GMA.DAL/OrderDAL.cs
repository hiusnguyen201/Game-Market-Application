namespace GMA.DAL;
using GMA.Models;
using System.Data;
using MySql.Data.MySqlClient;

public class OrderDAL
{
    public int CreateOrder(Order order)
    {
        int result = 0;
        string insertQuery = "create_order";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(insertQuery, DBHelper.GetConnection());
            command.CommandText = insertQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aid", order.OrderAccount.AccountId);
            command.Parameters.AddWithValue("@otp", order.TotalPrice);
            command.Parameters.AddWithValue("@os", order.Status);
            command.Parameters["@aid"].Direction = ParameterDirection.Input;
            command.Parameters["@otp"].Direction = ParameterDirection.Input;
            command.Parameters["@os"].Direction = ParameterDirection.Input;
            command.Parameters.Add(new MySqlParameter("@aid", MySqlDbType.Int32));
            command.Parameters["@aid"].Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();
            result = Convert.ToInt32(command.Parameters["@aid"].Value);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
        }
        finally
        {
            DBHelper.CloseConnection();
        }
        return result;
    }
}