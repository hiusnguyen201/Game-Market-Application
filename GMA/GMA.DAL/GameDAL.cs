namespace GMA.DAL;
using GMA.Models;
using System.Data;
using MySql.Data.MySqlClient;

public class GameDAL
{
    private Game game = null;
    private List<Game> games = null;
    private Game Get(MySqlDataReader reader)
    {
        Game game = new Game();
        game.GameId = reader.GetInt16("game_ID");
        game.Name = reader.GetString("game_Name");
        game.Desc = reader.GetString("game_Desc");
        game.Rating = reader.GetFloat("game_Rating");
        game.Size = reader.GetString("game_Size");
        game.Discount = reader.GetFloat("game_Discount");
        game.ReleaseDate = reader.GetDateTime("game_ReleaseDate");
        game.GamePublisher.PublisherID = reader.GetInt16("publisher_ID");
        game.GamePublisher.PublisherName = reader.GetString("publisher_Name");
        game.GameGenres.Add(new Genre(reader.GetInt16("genre_ID"),reader.GetString("genre_Name")));
        return game;
    }


    public Game GetById(int id)
    {
        Game game = null;
        string selectQuery = "get_game_by_id";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@gid", id);
            command.Parameters["@gid"].Direction = ParameterDirection.Input;
            MySqlDataReader gameReader = command.ExecuteReader();
            if (gameReader.Read())
            {
                game = Get(gameReader);
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
        return game;
    }

    public List<Game> GetAll()
    {
        List<Game> games = new List<Game>();
        Game game = null;
        string selectQuery = "Select * From get_all_games";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            MySqlDataReader gameReader = command.ExecuteReader();
            if (gameReader.Read())
            {
                game = Get(gameReader);
                games.Add(game);
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
        return games;
    }
}