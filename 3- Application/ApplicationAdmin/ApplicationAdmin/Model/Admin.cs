using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.Model
{
    public class Admin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Admin(int id, string name, string role, string email, string password)
        {
            Id = id;
            Name = name;
            Role = role;
            Email = email;
            Password = password;
        }
    }
}
