namespace GMA.DAL;
using GMA.Models;
using System.Data;
using MySql.Data.MySqlClient;

public class OrderDAL
{
    private Game game = null;
    private List<Game> games = null;
    private AccountDAL accountDAL = new AccountDAL();
    private GameDAL gameDAL = new GameDAL();

    private Order Get(MySqlDataReader reader)
    {
        Order order = new Order();
        order.OrderId = reader.GetInt16("order_ID");
        order.OrderAccount = new Account(reader.GetInt16("acc_ID"));
        order.TotalPrice = reader.GetDouble("order_TotalPrice");
        order.OrderDate = reader.GetDateTime("order_CreateDate");
        order.Status = reader.GetInt16("order_Status");
        string gameIds = reader.GetString("game_ID");   
        string[] splitGameIds = gameIds.Split(',');
        for (int i = 0; i < splitGameIds.Length; i++)
        {
            if (int.TryParse(splitGameIds[i], out int gameID))
            {
                order.OrderGames.Add(new Game(gameID));
            }
        }
        return order;
    }

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

    public int CreateOrderDetails(int orderID, int gameID)
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
            command.Parameters["@oid"].Direction = ParameterDirection.Input;
            command.Parameters["@gid"].Direction = ParameterDirection.Input;
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

    public List<Order> GetAll(int aid)
    {
        List<Order> orders = new List<Order>();
        Order order = null;
        try
        {
            DBHelper.OpenConnection();
            string selectString = "get_all_order";
            MySqlCommand command = new MySqlCommand(selectString, DBHelper.GetConnection());
            command.CommandText = selectString;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aid", aid);
            command.Parameters["@aid"].Direction = ParameterDirection.Input;
            MySqlDataReader orderReader = command.ExecuteReader();
            while (orderReader.Read())
            {
                order = Get(orderReader);
                orders.Add(order);
            }
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
            Console.ReadKey();
        }
        finally
        {
            DBHelper.CloseConnection();
        }

        order.OrderAccount = accountDAL.GetById(order.OrderAccount.AccountId);

        for (int i = 0; i < orders.Count; i++)
        {
            Order newOrder = orders[i];
            for (int j = 0; j < newOrder.OrderGames.Count; j++)
            {
                Game game = newOrder.OrderGames[j];
                Game updatedGame = gameDAL.GetById(game.GameId);

                if (updatedGame != null)
                {
                    newOrder.OrderGames[j] = updatedGame;
                }
            }
        }

        return orders;
    }

    public Order GetById(int oid)
    {
        Order order = null;
        try
        {
            DBHelper.OpenConnection();
            string selectQuery = "get_order_by_id";
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@oid", oid);
            command.Parameters["@oid"].Direction = ParameterDirection.Input;
            MySqlDataReader orderReader = command.ExecuteReader();
            if (orderReader.Read())
            {
                order = Get(orderReader);
            }
        }
        catch (Exception ex)
        {
            Console.Write(ex.ToString());
            Console.ReadKey();
        }
        finally
        {
            DBHelper.CloseConnection();
        }

        order.OrderAccount = accountDAL.GetById(order.OrderAccount.AccountId);

        for (int j = 0; j < order.OrderGames.Count; j++)
        {
            Game game = order.OrderGames[j];
            Game updatedGame = gameDAL.GetById(game.GameId);

            if (updatedGame != null)
            {
                order.OrderGames[j] = updatedGame;
            }
        }

        return order;
    }
}