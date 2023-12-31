namespace GMA.DAL;
using GMA.Models;
using System.Data;
using MySql.Data.MySqlClient;

public class GenreDAL
{
    private Genre genre = null;
    private Genre Get(MySqlDataReader reader)
    {
        Genre genre = new Genre();
        genre.GenreId = reader.GetInt16("id");
        genre.GenreName = reader.GetString("name");
        return genre;
    }

    // 1 Hàm Get All
    public Genre GetById(int id)
    {
        Genre genre = null;
        string selectQuery = "get_genre_by_id";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("genid", id);
            command.Parameters["genid"].Direction = ParameterDirection.Input;
            MySqlDataReader genreReader = command.ExecuteReader();
            while (genreReader.Read())
            {
                genre = Get(genreReader);
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
        return genre;
    }

    public List<Genre> GetAll()
    {
        List<Genre> genres = new List<Genre>();
        Genre genre = null;
        string selectQuery = "get_all_genres";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
            command.CommandType = CommandType.StoredProcedure;
            MySqlDataReader genreReader = command.ExecuteReader();
            while (genreReader.Read())
            {
                genre = Get(genreReader);
                genres.Add(genre);
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
        return genres;
    }
}