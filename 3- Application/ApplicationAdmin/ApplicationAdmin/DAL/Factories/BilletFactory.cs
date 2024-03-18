using ApplicationAdmin.Model;
using ApplicationAdmin.View;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.DAL.Factories
{
    public class BilletFactory
    {
        Dal DAL = new Dal();
        private Billet CreateFromReader(MySqlDataReader reader)
        {
            int Id = (int)reader["Id"];
            string Banc = reader["Banc"].ToString() ?? string.Empty;
            string Range = reader["RangeSiege"].ToString() ?? string.Empty;
            int IdClient = (int)reader["IdClient"];
            int IdEvenement = (int)reader["IdEvenement"];

            return new Billet(Id, Banc, Range, IdClient, IdEvenement);
        }



        public IEnumerable<Billet> GetAll()
        {
            List<Billet> billets = new List<Billet>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Billet;";
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    Billet billet = CreateFromReader(mySqlDataReader);
                    billets.Add(billet);
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

            return billets;
        }

        public Billet Getbyid(int id)
        {
            Billet? billet = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Billet WHERE id = @Id;";
                commande.Parameters.AddWithValue("@Id", id);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    billet = CreateFromReader(mySqlDataReader);
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

            return billet;
        }
        
        public Billet Getbyplace(string banc, string range,int idEvenement)
        {
            Billet? billet = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Billet WHERE RangeSiege = @range AND Banc = @banc AND IdEvenement = @idEvenement;";
                commande.Parameters.AddWithValue("@banc", banc);
                commande.Parameters.AddWithValue("@range", range);
                commande.Parameters.AddWithValue("@idEvenement", idEvenement);

                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    billet = CreateFromReader(mySqlDataReader);
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

            return billet;
        }

        public List<Billet> GetAllBilletsEvenement(int idEvenement)
        {
            List<Billet>? billets = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Billet WHERE id = @IdEvenement;";
                commande.Parameters.AddWithValue("@Id", idEvenement);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    billets.Add(CreateFromReader(mySqlDataReader));
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

            return billets;
        }
    }
}
