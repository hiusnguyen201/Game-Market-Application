namespace GMA.DAL;
using GMA.Models;
using System.Data;
using MySql.Data.MySqlClient;

public class AccountDAL
{
    private Account account = null;

    private Account Get(MySqlDataReader reader)
    {
        Account account = new Account();
        account.AccountId = reader.GetInt16("id");
        account.Username = reader.GetString("username");
        account.Password = reader.GetString("password");
        account.Realname = reader.GetString("realname");
        account.Email = reader.GetString("email");
        account.Address = reader.GetString("address");
        account.Money = reader.GetDouble("money");
        account.CreatedAt = reader.GetDateTime("created_at");
        account.UpdatedAt = reader.GetDateTime("updated_at");
        return account;
    }

    public Account GetById(int aid)
    {
        Account account = null;
        string selectQuery = "get_acc_by_id";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aid", aid);
            command.Parameters["@aid"].Direction = ParameterDirection.Input;
            MySqlDataReader accountReader = command.ExecuteReader();
            if (accountReader.Read())
            {
                account = Get(accountReader);
            }
        }
        catch (Exception e)
        {
            Console.Write(e);
            Console.ReadKey();
        }
        finally
        {
            DBHelper.CloseConnection();
        }
        return account;
    }

    public Account GetByUsername(string username)
    {
        Account account = null;
        string selectQuery = "get_acc_by_username";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aun", username);
            command.Parameters["@aun"].Direction = ParameterDirection.Input;
            MySqlDataReader accountReader = command.ExecuteReader();
            if (accountReader.Read())
            {
                account = Get(accountReader);
            }
        }
        catch (Exception e)
        {
            Console.Write(e);
            Console.ReadKey();
        }
        finally
        {
            DBHelper.CloseConnection();
        }
        return account;
    }

    public Account GetAccountLogin(string username, string password)
    {
        Account account = null;
        string selectQuery = "get_account_login";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aun", username);
            command.Parameters.AddWithValue("@apw", password);
            command.Parameters["@aun"].Direction = ParameterDirection.Input;
            command.Parameters["@apw"].Direction = ParameterDirection.Input;
            MySqlDataReader accountReader = command.ExecuteReader();
            if (accountReader.Read())
            {
                account = Get(accountReader);
            }
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
        return account;
    }

    public Account GetByEmail(string email)
    {
        Account account = null;
        string selectQuery = "get_acc_by_email";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ae", email);
            command.Parameters["@ae"].Direction = ParameterDirection.Input;
            MySqlDataReader accountReader = command.ExecuteReader();
            if (accountReader.Read())
            {
                account = Get(accountReader);
            }
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
        return account;
    }

    public int SaveAccount(Account account)
    {
        int result = 0;
        string insertQuery = "create_account";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(insertQuery, DBHelper.GetConnection());
            command.CommandText = insertQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aun", account.Username);
            command.Parameters.AddWithValue("@apw", account.Password);
            command.Parameters.AddWithValue("@arn", account.Realname);
            command.Parameters.AddWithValue("@ae", account.Email);
            command.Parameters.AddWithValue("@aa", account.Address);
            command.Parameters["@aun"].Direction = ParameterDirection.Input;
            command.Parameters["@apw"].Direction = ParameterDirection.Input;
            command.Parameters["@arn"].Direction = ParameterDirection.Input;
            command.Parameters["@ae"].Direction = ParameterDirection.Input;
            command.Parameters["@aa"].Direction = ParameterDirection.Input;
            command.Parameters.Add(new MySqlParameter("@aid", MySqlDbType.Int32));
            command.Parameters["@aid"].Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();
            result = Convert.ToInt32(command.Parameters["@aid"].Value);
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

    public int UpdateAccountMoney(int accId, double mn)
    {
        int result = 0;
        string updateQuery = "update_acc_Money";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(updateQuery, DBHelper.GetConnection());
            command.CommandText = updateQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@aid", accId);
            command.Parameters.AddWithValue("@mn", mn);
            command.Parameters["@aid"].Direction = ParameterDirection.Input;
            command.Parameters["@mn"].Direction = ParameterDirection.Input;
            command.Parameters.Add(new MySqlParameter("@res", MySqlDbType.Int32));
            command.Parameters["@res"].Direction = ParameterDirection.Output;
            command.ExecuteNonQuery();
            result = Convert.ToInt32(command.Parameters["@res"].Value);
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