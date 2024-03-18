using ApplicationAdmin.Model;
using ApplicationAdmin.Model_View;
using ApplicationAdmin.View;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace ApplicationAdmin.DAL.Factories
{
    public class PanierFactory
    {
        Dal DAL = new Dal();
        private Model.Panier CreateFromReader(MySqlDataReader reader)
        {
            int Id = (int)reader["Id"];
            bool Paye = (bool)reader["Paye"];
            int IdClient = (int)reader["IdClient"];
            DateTime? datePaye = (DateTime)reader["DatePaye"];

            return new Panier(Id, Paye, IdClient, datePaye);
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

        public Panier GetPanierClient(int id)
        {
            Panier? panier = null;
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

        
    }
}
