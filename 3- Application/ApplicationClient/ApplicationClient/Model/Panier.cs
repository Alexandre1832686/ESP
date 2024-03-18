using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClient.Model
{
    public class Panier
    {
        public int Id;
        public bool Paye;
        public DateTime DatePaye;
        public int IdClient;

        public Panier(int id, bool paye, int idClient)
        {
            Id = id;
            Paye = paye;
            this.IdClient = idClient;
        }
    }
}
