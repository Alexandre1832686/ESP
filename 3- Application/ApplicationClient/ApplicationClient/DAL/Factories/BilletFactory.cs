using ApplicationClient.Model;
using ApplicationClient.View;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClient.DAL.Factories
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

        public bool CreerBillet(string banc, string range, int evenement, int client)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "insert into billet(Banc, IdClient, IdEvenement, RangeSiege) values(@Banc, @IdClient, @IdEvenement, @Range)";
                commande.Parameters.AddWithValue("@Banc", banc);
                commande.Parameters.AddWithValue("@IdClient", client);
                commande.Parameters.AddWithValue("@IdEvenement", evenement);
                commande.Parameters.AddWithValue("@Range", range);
                commande.ExecuteReader();
            }
            catch (Exception e)
            {
                ErrorWindow ErWin = new ErrorWindow(e.Message);
                ErWin.Show();
                return false;
            }
            finally
            {

                mySqlDataReader?.Close();
                mySqlCnn?.Close();
                
            }
            return true;
        }

        public void AjouterBilletPanier(int idBillet,int idPanier)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "insert into panierbillet(idBillet, idPanier) values(@idBillet, @idPanier)";
                commande.Parameters.AddWithValue("@idBillet", idBillet);
                commande.Parameters.AddWithValue("@idPanier", idPanier);

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

        public bool BilletExist(Evenement ev, string range,string place)
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
                commande.Parameters.AddWithValue("@banc", place);
                commande.Parameters.AddWithValue("@range", range);
                commande.Parameters.AddWithValue("@idEvenement", ev.Id);

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
            if(billet != null) { return true; } else { return false; }
        }


        public List<Billet>? ListBilletsEvenement(Evenement ev)
        {
            
            List<Billet>? billets = new List<Billet>();

            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Billet WHERE IdEvenement = @idEvenement;";
                commande.Parameters.AddWithValue("@idEvenement", ev.Id);

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
