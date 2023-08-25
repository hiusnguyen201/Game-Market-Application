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
        game.GameId = reader.GetInt16("game_id");
        game.Name = reader.GetString("game_name");
        game.Desc = reader.GetString("description");
        game.Price = reader.GetDouble("price");
        game.Rating = reader.GetInt16("rating");
        game.Size = reader.GetString("size");
        game.CreatedAt = reader.GetDateTime("created_at");
        game.GamePublisher.PublisherID = reader.GetInt16("publisher_id");
        game.GamePublisher.PublisherName = reader.GetString("publisher_name");
        string genreIDs = reader.GetString("genre_id");
        string genreNames = reader.GetString("genre_name");
        string[] splitGenreIds = genreIDs.Split(',');
        string[] splitGenreNames = genreNames.Split(',');
        for(int i = 0; i < splitGenreIds.Length; i++)
        {
            if(int.TryParse(splitGenreIds[i], out int genreID))
            {
                game.GameGenres.Add(new Genre(genreID, splitGenreNames[i]));
            }
        }
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

    // 1 Hàm GetByCateName
    public List<Game> GetByGenIdKey(string kw, int gid)
    {
         List<Game> games = new List<Game>();
        Game game = null;
        string selectQuery = "get_game_by_GenIdKey";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@kw", kw);
            command.Parameters.AddWithValue("@genid", gid);
            command.Parameters["@kw"].Direction = ParameterDirection.Input;
            command.Parameters["@genid"].Direction = ParameterDirection.Input;
            MySqlDataReader gameReader = command.ExecuteReader();
            while (gameReader.Read())
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