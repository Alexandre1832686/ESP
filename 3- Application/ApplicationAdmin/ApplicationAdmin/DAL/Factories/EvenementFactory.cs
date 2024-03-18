using ApplicationAdmin.Model;
using ApplicationAdmin.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.DAL.Factories
{
    public class EvenementFactory
    {
        Dal DAL = new Dal();
        private Evenement CreateFromReader(MySqlDataReader reader)
        {
            int id = (int)reader["Id"];
            string artiste = reader["Artiste"].ToString() ?? string.Empty;
            string nomSpectacle = reader["NomSpectacle"].ToString() ?? string.Empty;
            string image = reader["ImageSpectacle"].ToString() ?? string.Empty;
            double CoutEvenement = (double)reader["CoutEvenement"];

            DateTime? date = null;
            if (!reader.IsDBNull(reader.GetOrdinal("Date")))
            {
                date = (DateTime)reader["Date"];
            }

            DateOnly dateonly = new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day);

            DateTime? date2 = null;
            if (!reader.IsDBNull(reader.GetOrdinal("LimiteAchat")))
            {
                date2 = (DateTime)reader["LimiteAchat"];
            }

            DateOnly dateonly2 = new DateOnly(date2.Value.Year, date2.Value.Month, date2.Value.Day);


            string heure = reader["Heure"].ToString() ?? string.Empty;
            string cout = reader["Cout"].ToString() ?? string.Empty;

            return new Evenement(id, artiste, nomSpectacle, CoutEvenement, image, dateonly, dateonly2, heure,cout);
        }

        public List<Evenement> GetAll()
        {
            List<Evenement> evenements = new List<Evenement>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Evenement;";
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    Evenement evenement = CreateFromReader(mySqlDataReader);
                    evenements.Add(evenement);
                }
            }
            catch (Exception e)
            {
                ErrorWindow ErWin = new ErrorWindow(e.Message);
                ErWin.Show();
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return evenements;
        }

        public List<Evenement> GetAllAchetable()
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);

            List<Evenement> evenements = new List<Evenement>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Evenement WHERE LimiteAchat>@now;";
                commande.Parameters.AddWithValue("@now", now);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    Evenement evenement = CreateFromReader(mySqlDataReader);
                    evenements.Add(evenement);
                }
            }
            catch (Exception e)
            {
                ErrorWindow ErWin = new ErrorWindow(e.Message);
                ErWin.Show();
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return evenements;
        }

        public Evenement Get(int id)
        {
            Evenement? evenement = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Evenement WHERE id = @Id;";
                commande.Parameters.AddWithValue("@Id", id);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    evenement = CreateFromReader(mySqlDataReader);
                }
            }
            catch (Exception e)
            {
                ErrorWindow ErWin = new ErrorWindow(e.Message);
                ErWin.Show();
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return evenement;
        }

        public void Add(string Artiste,string NomSpectacle,string ImageSpectacle,DateOnly Date, DateOnly Date2, string Heure, string Cout, string CoutEvenement)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                
                commande.CommandText = "Insert into Evenement (Artiste, NomSpectacle, CoutEvenement, ImageSpectacle, Date,LimiteAchat, Cout,Heure) values (@artiste,@nomspectacle,@CoutEvenement,@imagespectacle,@date,@date2,@cout,@Heure);";
                commande.Parameters.AddWithValue("@artiste", Artiste);
                commande.Parameters.AddWithValue("@nomspectacle", NomSpectacle);
                commande.Parameters.AddWithValue("@imagespectacle", ImageSpectacle);
                commande.Parameters.AddWithValue("@CoutEvenement", CoutEvenement);
                commande.Parameters.AddWithValue("@date", Date);
                commande.Parameters.AddWithValue("@date2", Date2);
                commande.Parameters.AddWithValue("@Cout", Cout);
                commande.Parameters.AddWithValue("@Heure", Heure);

                commande.ExecuteReader();
            }
            catch (Exception e)
            {
                ErrorWindow ErWin = new ErrorWindow(e.Message);
                ErWin.Show();
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }
        }

        public void Update(int id, string Artiste, string NomSpectacle, string ImageSpectacle, DateOnly Date, DateOnly Date2, string Heure, string Cout, string CoutEvenement)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();

                commande.CommandText = "Update Evenement set Artiste=@Artiste, NomSpectacle=@NomSpectacle, CoutEvenement=@CoutEvenement, ImageSpectacle=@ImageSpectacle, Date=@Date, LimiteAchat=@Date2, Cout=@Cout,Heure=@Heure Where Id=@Id;";
                commande.Parameters.AddWithValue("@Artiste", Artiste);
                commande.Parameters.AddWithValue("@NomSpectacle", NomSpectacle);
                commande.Parameters.AddWithValue("@ImageSpectacle", ImageSpectacle);
                commande.Parameters.AddWithValue("@CoutEvenement", CoutEvenement);
                commande.Parameters.AddWithValue("@Date", Date);
                commande.Parameters.AddWithValue("@Date2", Date2);
                commande.Parameters.AddWithValue("@Cout", Cout);
                commande.Parameters.AddWithValue("@Heure", Heure);
                commande.Parameters.AddWithValue("@Id", id);


                commande.ExecuteReader();
            }
            catch (Exception e)
            {
                ErrorWindow ErWin = new ErrorWindow(e.Message);
                ErWin.Show();
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }
        }

        public void Supprimer(int id)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();

                commande.CommandText = "Delete from Evenement Where Id=@Id;";
                commande.Parameters.AddWithValue("@Id", id);

                commande.ExecuteReader();
            }
            catch (Exception e)
            {
                ErrorWindow ErWin = new ErrorWindow(e.Message);
                ErWin.Show();
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }
        }
    }
}
