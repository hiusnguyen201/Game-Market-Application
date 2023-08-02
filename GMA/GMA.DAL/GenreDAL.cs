namespace GMA.DAL;
using GMA.Models;
using System.Data;
using MySql.Data.MySqlClient;

public class GenreDAL
{
    public Genre Get(MySqlDataReader reader)
    {
        Genre genre = new Genre();
        genre.GenreID = reader.GetInt16("genre_ID");
        genre.Name = reader.GetString("genre_name");
        return genre;
    }

    // 1 Hàm Get All
    public List<Genre> GetAll()
    {
        List<Genre> genres = new List<Genre>();
        Genre genre = null;
        string selectQuery = "SELECT * FROM genres";
        try
        {
            DBHelper.OpenConnection();
            MySqlCommand command = new MySqlCommand(selectQuery, DBHelper.GetConnection());
            command.CommandText = selectQuery;
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