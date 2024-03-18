using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClient.Model
{
    public class Admin
    {
        public string Name;
        public string Role;
        public string Email;
        public string Password;

        public Admin(string name, string role, string email, string password)
        {
            Name = name;
            Role = role;
            Email = email;
            Password = password;
        }
    }
}
