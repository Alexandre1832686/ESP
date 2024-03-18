using ApplicationAdmin.Model;
using ApplicationAdmin.Model_View;
using ApplicationAdmin.View;
using ApplicationAdmin.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using MySql.Data.MySqlClient;

namespace ApplicationAdmin.DAL.Factories
{
    public class AdminFactory
    {
        Dal DAL = new Dal();
        private Admin CreateFromReader(MySqlDataReader reader)
        {
            int id = (int)reader["Id"];
            string? name = reader["Name"].ToString() ?? string.Empty;
            string? role = reader["Role"].ToString() ?? string.Empty;
            string email = reader["Email"].ToString() ?? string.Empty;
            string password = reader["Password"].ToString() ?? string.Empty;

            return new Admin(id, name, role ,email, password);
        }

        public Admin Get(int id)
        {
            Admin? a = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Admin WHERE id = @Id;";
                commande.Parameters.AddWithValue("@Id", id);
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    a = CreateFromReader(mySqlDataReader);
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

            return a;
        }

        public List<Admin> GetAll()
        {
            List<Admin> admins = new List<Admin>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();

                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Admin;";
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    Admin admin = CreateFromReader(mySqlDataReader);
                    admins.Add(admin);
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

            return admins;
        }

        public Admin CheckConnection(string username, string password)
        {
            Admin a = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "SELECT * FROM Admin;";
                mySqlDataReader = commande.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    if (mySqlDataReader["Name"].ToString() == username && mySqlDataReader["Password"].ToString() == Hasher.HashPassword(password))
                    {
                        a = CreateFromReader(mySqlDataReader);
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

            return a;
        }

        public void ChangeName(Admin a, string nom)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "UPDATE Admin SET Name = @Name WHERE Id = @Id ;";
                commande.Parameters.AddWithValue("@Name", nom);
                commande.Parameters.AddWithValue("@Id", a.Id);
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

        public void CreerCompte(string username, string password, string email, string role)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "insert into admin(Name, Password, Email, Role) values(@username, @password, @email, @role)";
                commande.Parameters.AddWithValue("@username", username);
                commande.Parameters.AddWithValue("@role", role);
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

        public void DeleteAccount(string nom)
        {
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConectionString);
                mySqlCnn.Open();
                MySqlCommand commande = mySqlCnn.CreateCommand();
                commande.CommandText = "DELETE FROM Admin WHERE Name = @Name;";
                commande.Parameters.AddWithValue("@Name", nom);
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
