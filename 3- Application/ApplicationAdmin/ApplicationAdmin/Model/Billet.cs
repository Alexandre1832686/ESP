using ApplicationAdmin.DAL;
using ApplicationAdmin.Model_View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.Model
{
    public class Billet
    {
        public int Id { get; set; }
        public string Banc { get; set; }
        public string Range { get; set; }
        
        public int IdClient { get; set; }
        public int IdEvenement { get; set; }
        public Evenement Evenement { get; set; }
        public double Prix { get; set; }

        public Billet(int id, string banc, string range, int idclient, int idevenement)
        {
            this.Id = id;
            this.Banc = banc;
            this.Range = range;
            this.IdClient = idclient;
            this.IdEvenement = idevenement;
            Dal dal = new Dal();
            this.Evenement = dal.EvenementFactory.Get(idevenement);
            Prix = AppVM.GetPrixBillet(this.Evenement, this.Range);
        }
    }
}
