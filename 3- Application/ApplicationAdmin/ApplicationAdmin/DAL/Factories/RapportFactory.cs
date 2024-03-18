using ApplicationAdmin.Model;
using ApplicationAdmin.Model_View;
using ApplicationAdmin.View;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.DAL.Factories
{
    public class RapportFactory
    {
        Dal DAL = new Dal();
        private Billet CreateBilletFromReader(MySqlDataReader reader)
        {
            int Id = (int)reader["Id"];
            string Banc = reader["Banc"].ToString() ?? string.Empty;
            string Range = reader["RangeSiege"].ToString() ?? string.Empty;
            int IdClient = (int)reader["IdClient"];
            int IdEvenement = (int)reader["IdEvenement"];

            return new Billet(Id, Banc, Range, IdClient, IdEvenement);
        }

        private Evenement CreateEvenementFromReader(MySqlDataReader reader)
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

            return new Evenement(id, artiste, nomSpectacle, CoutEvenement, image, dateonly, dateonly2, heure, cout);
        }

        public Model.Rapport GetRapportEvenement(int idEvenement)
        {
            List<Billet>? billets = new List<Billet>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;


            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM vuebilletspayes Where idEvenement = @idEvenement;";
                commande.Parameters.AddWithValue("@idEvenement", idEvenement);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    billets.Add(CreateBilletFromReader(mySqlDataReader));
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

            Model.Rapport r = new Model.Rapport();
            r.nbBillet = 0;
            r.Revenu = 0;
            int placeprise = 0;

            foreach (Billet b in billets)
            {
                r.nbBillet += 1;
                
                r.Revenu += AppVM.GetPrixBillet(b.Evenement, b.Range);
                placeprise += 1;
            }
            r.Cout = DAL.EvenementFactory.Get(idEvenement).CoutEvenement;
            r.Remplie = Math.Round(placeprise/772d, 2) ;
            r.Profit = r.Revenu - r.Cout;
            return r;
        }

        public Model.Rapport GetRapportClient(int idClient)
        {
            List<Billet>? billets = new List<Billet>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM vuebilletspayes Where idClient = @idClient;";
                commande.Parameters.AddWithValue("@idClient", idClient);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    billets.Add(CreateBilletFromReader(mySqlDataReader));
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

            Model.Rapport r = new Model.Rapport();

            r.nbBillet = 0;
            r.Revenu = 0;

            foreach (Billet b in billets)
            {
                r.nbBillet += 1;
                r.Revenu += AppVM.GetPrixBillet(b.Evenement, b.Range);
            }

            return r;
        }

        public Model.Rapport GetRapportDate(DateOnly min, DateOnly max)
        {
            List<Billet>? billets = new List<Billet>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM vuebilletspayes Where DatePaye >= @min AND DatePaye <= @max;";
                commande.Parameters.AddWithValue("@min", min);
                commande.Parameters.AddWithValue("@max", max);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    billets.Add(CreateBilletFromReader(mySqlDataReader));
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

            Model.Rapport r = new Model.Rapport();
            r.nbBillet = 0;
            r.Revenu = 0;
            r.Cout = 0;
            int placeprise = 0;
            List<Evenement> listevenements = DAL.EvenementFactory.GetAll();

            foreach (Evenement e in listevenements)
            {
                r.Cout += e.CoutEvenement;
            }

            foreach (Billet b in billets)
            {
                
                r.nbBillet += 1;
               
                r.Revenu += AppVM.GetPrixBillet(b.Evenement, b.Range);
                placeprise += 1;
            }

            r.Remplie = placeprise / (772d*listevenements.Count());
            r.Profit = r.Revenu - r.Cout;
            return r;
        }
    }
}
