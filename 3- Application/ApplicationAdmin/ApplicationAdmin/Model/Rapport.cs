using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.Model
{
    public class Rapport
    {
        public double Remplie { get; set; }
        public int nbBillet { get; set; }
        public double Revenu { get; set; }
        public double Profit { get; set; }
        public double Cout { get; set; }

        public Rapport() { }
        public Rapport(double remplie, int nbBillet, double revenu, double profit, double cout)
        {
            Remplie = remplie;
            this.nbBillet = nbBillet;
            Revenu = revenu;
            Profit = profit;
            Cout = cout;
        }
    }
}
