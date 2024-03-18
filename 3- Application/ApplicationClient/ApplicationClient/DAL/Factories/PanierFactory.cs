using ApplicationClient.Model;
using ApplicationClient.Model_View;
using ApplicationClient.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace ApplicationClient.DAL.Factories
{
    public class PanierFactory
    {
        Dal DAL = new Dal();
        private Model.Panier CreateFromReader(MySqlDataReader reader)
        {
            int Id = (int)reader["Id"];
            bool Paye = (bool)reader["Paye"];
            int IdClient = (int)reader["IdClient"];

            return new Model.Panier(Id, Paye, IdClient);
        }
        private Model.PanierBillet CreatePanierBilletFromReader(MySqlDataReader reader)
        {
            int Id = (int)reader["Id"];
            int IdBillet = (int)reader["IdBillet"];
            int IdPanier = (int)reader["IdPanier"];

            return new Model.PanierBillet(Id, IdBillet, IdPanier);
        }
        



        public IEnumerable<Model.Panier> GetAll()
        {
            List<Model.Panier> paniers = new List<Model.Panier>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Panier;";
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    Model.Panier panier = CreateFromReader(mySqlDataReader);
                    paniers.Add(panier);
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

            return paniers;
        }

        public Model.Panier GetPanierClient(int id)
        {
            Model.Panier? panier = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Panier WHERE idclient = @Id AND Paye = false;";
                commande.Parameters.AddWithValue("@Id", id);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    panier = CreateFromReader(mySqlDataReader);
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

            return panier;
        }

        public Model.Panier CreerPanier(int idClient)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "insert into Panier(idClient, Paye) values(@idClient, false)";
                commande.Parameters.AddWithValue("@idClient", idClient);
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
            return GetPanierClient(idClient);
        }

        public void RetirerBillet(int idBillet)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "DELETE FROM PanierBillet WHERE IdBillet = @IdBillet";
                commande.Parameters.AddWithValue("@IdBillet", idBillet);
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

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "DELETE FROM Billet WHERE Id = @IdBillet";
                commande.Parameters.AddWithValue("@IdBillet", idBillet);
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

        public List<Billet> GetAllBillets(int idClient)
        {
            List<Billet> billets = new List<Billet>();
            Model.Panier p = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Panier WHERE IdClient = @Id AND Paye = false;";
                commande.Parameters.AddWithValue("@Id", idClient);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    p = CreateFromReader(mySqlDataReader);
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

            if(p== null)
            {
                CreerPanier(idClient);
                try
                {
                    mySqlCnn = new MySqlConnection(DAL.ConectionString);
                    mySqlCnn.Open();

                    MySqlCommand commande = mySqlCnn.CreateCommand();
                    commande.CommandText = "SELECT * FROM Panier WHERE IdClient = @Id AND Paye = false;";
                    commande.Parameters.AddWithValue("@Id", idClient);
                    mySqlDataReader = commande.ExecuteReader();
                    while (mySqlDataReader.Read())
                    {
                        p = CreateFromReader(mySqlDataReader);
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
            }

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM PanierBillet WHERE IdPanier = @IdPanier;";
                commande.Parameters.AddWithValue("@IdPanier", p.Id);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    PanierBillet panierbillet = CreatePanierBilletFromReader(mySqlDataReader);
                    billets.Add(DAL.BilletFactory.Getbyid(panierbillet.IdBillet));
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

        public void PayerPanier(int idClient)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "Update Panier Set Paye = true, DatePaye = @date WHERE idClient = @idClient AND Paye = false";
                commande.Parameters.AddWithValue("@idClient", idClient);
                commande.Parameters.AddWithValue("@date", DateTime.Now);
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

        public List<Billet> GetHistorique(int idClient)
        {
            List<Billet> billets = new List<Billet>();
            List<Model.Panier> paniers = new List<Model.Panier>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Panier WHERE IdClient = @Id AND Paye = true;";
                commande.Parameters.AddWithValue("@Id", idClient);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    paniers.Add(CreateFromReader(mySqlDataReader));
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


            foreach(Model.Panier p in paniers)
            {
                try
                {
                    mySqlCnn = new MySqlConnection(DAL.ConectionString);
                    mySqlCnn.Open();

                    MySqlCommand commande = mySqlCnn.CreateCommand();
                    commande.CommandText = "SELECT * FROM PanierBillet WHERE IdPanier = @IdPanier;";
                    commande.Parameters.AddWithValue("@IdPanier", p.Id);
                    mySqlDataReader = commande.ExecuteReader();
                    while (mySqlDataReader.Read())
                    {
                        PanierBillet panierbillet = CreatePanierBilletFromReader(mySqlDataReader);
                        billets.Add(DAL.BilletFactory.Getbyid(panierbillet.IdBillet));
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
            }
 
            return billets;
        }
        
    }
}
