﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.Model
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }



        public Client(int id, string name, string email, string password ) 
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }

    }
}