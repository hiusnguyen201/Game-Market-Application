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
            command.Parameters.Add(new MySqlParameter("@oid", MySqlDbType.Int32));
            command.Parameters["@oid"].Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();
            result = Convert.ToInt32(command.Parameters["@oid"].Value);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            Console.ReadKey();
        }
        finally
        {
            DBHelper.CloseConnection();
        }
        return result;
    }

    public int CreateOrderDetails(int orderID, int gameID, double unitPrice)
    {
        int result = 0;
        string insertQuery = "create_order_details";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(insertQuery, DBHelper.GetConnection());
            command.CommandText = insertQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@oid", orderID);
            command.Parameters.AddWithValue("@gid", gameID);
            command.Parameters.AddWithValue("@up", unitPrice);
            command.Parameters["@oid"].Direction = ParameterDirection.Input;
            command.Parameters["@gid"].Direction = ParameterDirection.Input;
            command.Parameters["@up"].Direction = ParameterDirection.Input;
            command.Parameters.Add(new MySqlParameter("@id", MySqlDbType.Int32));
            command.Parameters["@id"].Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();
            result = Convert.ToInt32(command.Parameters["@id"].Value);
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            Console.ReadKey();
        }
        finally
        {
            DBHelper.CloseConnection();
        }
        return result;
    }
}