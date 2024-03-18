using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationClient.Model
{
    public class PanierBillet
    {
        public int Id;
        public int IdBillet;
        public int IdPanier;

        public PanierBillet(int id, int idBillet, int idPanier)
        {
            Id = id;
            IdBillet = idBillet;
            IdPanier = idPanier;
        }
    }
}
