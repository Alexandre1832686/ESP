using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.Model
{
    public class Panier
    {
        public int Id;
        public bool Paye;
        public DateTime? DatePaye;
        public int IdClient;

        public Panier(int id, bool paye, int idClient, DateTime? datePaye)
        {
            Id = id;
            Paye = paye;
            this.IdClient = idClient;
            DatePaye = datePaye;
    }
    }
}
