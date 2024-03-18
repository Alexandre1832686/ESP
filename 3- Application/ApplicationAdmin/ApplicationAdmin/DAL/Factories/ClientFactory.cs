using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationAdmin.Model;
using System.Windows;
using ApplicationAdmin.View;
using ApplicationAdmin.Model_View;

namespace ApplicationAdmin.DAL.Factories
{
    public class ClientFactory
    {
        Dal DAL = new Dal();
        private Client CreateFromReader(MySqlDataReader reader)
        {
            int id = (int)reader["Id"];
            string? name = reader["Name"].ToString() ?? string.Empty;
            string email = reader["Email"].ToString() ?? string.Empty;
            string password = reader["Password"].ToString() ?? string.Empty;

            return new Client(id, name, email, password);
        }



        public List<Client> GetAll()
        {
            List<Client> clients = new List<Client>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Client;";
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    Client client = CreateFromReader(mySqlDataReader);
                    clients.Add(client);
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

            return clients;
        }

        public Client Get(int id)
        {
            Client? client = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Client WHERE id = @Id;";
                commande.Parameters.AddWithValue("@Id", id);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                     client = CreateFromReader(mySqlDataReader);
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

            return client;
        }

        public Client CheckConnection(string username, string password)
        {
            Client client = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Client;";
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    if (mySqlDataReader["Name"].ToString() == username && mySqlDataReader["Password"].ToString() == Hasher.HashPassword(password))
                    {
                        client = CreateFromReader(mySqlDataReader);
                    }
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

            return client;
        }

        public string CheckInscription(string username, string email)
        { 
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;
            string reponse = "Valide";
            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Client;";
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    if (mySqlDataReader["Name"].ToString() == username )
                    {
                        reponse = "Nom existant";
                    }

                    if(mySqlDataReader["Email"].ToString() == email)
                    {
                        reponse = "Email existant";
                    }
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

            return reponse;
        }

        public void CreerCompte(string username, string password, string email)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "insert into client(Name, Password, Email) values(@username, @password, @email)";
                commande.Parameters.AddWithValue("@username", username);
                commande.Parameters.AddWithValue("@password", Hasher.HashPassword(password));
                commande.Parameters.AddWithValue("@email", email);
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

        public void ChangeName(Client c, string nom)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "UPDATE Client SET Name = @Name WHERE Id = @Id ;";
                commande.Parameters.AddWithValue("@Name", nom);
                commande.Parameters.AddWithValue("@Id", c.Id);
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
