using ApplicationClient.Model;
using ApplicationClient.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClient.DAL.Factories
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
            

            DateTime? date = null;
            if (!reader.IsDBNull(reader.GetOrdinal("Date")))
            {
                date = (DateTime)reader["Date"];
            }

            DateOnly dateonly = new DateOnly(date.Value.Year, date.Value.Month, date.Value.Day);

            


            string heure = reader["Heure"].ToString() ?? string.Empty;
            string cout = reader["Cout"].ToString() ?? string.Empty;

            return new Evenement(id, artiste, nomSpectacle, image, dateonly, heure, cout);
        }



        public List<Evenement> GetAll()
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

        

        public List<Evenement> GetAllfilter(string text)
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
                commande.CommandText = "SELECT * FROM Evenement WHERE NomSpectacle LIKE '%" + text + "%' OR Artiste LIKE'%" + text + "%' AND LimiteAchat>@now;";
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
    }
}
