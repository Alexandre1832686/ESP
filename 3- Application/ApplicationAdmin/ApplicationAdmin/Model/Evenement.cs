using ApplicationAdmin.Model_View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.Model
{
    public class Evenement
    {
        public int Id { get; set; }
        public string Artiste { get; set; }
        public string NomSpectacle { get; set; }
        public string Image { get; set; }
        public DateOnly Date { get; set; }
        public DateOnly DateLimite { get; set; }
        public double CoutEvenement { get; set; }
        public string Heure { get; set; }
        public string Cout { get; set; }

        string[] tableRange;

        public List<Place[]> salle;

        public Evenement(int id, string artiste, string nomSpectacle, double coutEvenement, string image, DateOnly date, DateOnly dateLimite, string Heure, string Cout)
        {
            salle = new List<Place[]>();
            tableRange = new string[25];
            tableRange[0] = "A";
            tableRange[1] = "B";
            tableRange[2] = "C";
            tableRange[3] = "D";
            tableRange[4] = "E";
            tableRange[5] = "F";
            tableRange[6] = "G";
            tableRange[7] = "H";
            tableRange[8] = "I";
            tableRange[9] = "J";
            tableRange[10] = "K";
            tableRange[11] = "L";
            tableRange[12] = "M";
            tableRange[13] = "N";
            tableRange[14] = "O";
            tableRange[15] = "P";
            tableRange[16] = "Q";
            tableRange[17] = "AM";
            tableRange[18] = "BM";
            tableRange[19] = "CM";
            tableRange[20] = "DM";
            tableRange[21] = "AA";
            tableRange[22] = "BB";
            tableRange[23] = "CC";
            tableRange[24] = "DD";

            this.Id = id;
            this.Artiste = artiste;
            this.NomSpectacle = nomSpectacle;
            this.Image = image;
            this.Date = date;
            this.DateLimite = dateLimite;
            this.Heure = Heure;
            this.CoutEvenement = coutEvenement;
            this.Cout = Cout;

            for (int i = 0; i < 25; i++)
            {
                int max = AppVM.GetMaxRange(tableRange[i]);


                Place[] places = new Place[max];
                for (int j = 0; j < max; j++)
                {
                    Place p = new Place(j, AppVM.ConvertIndToRange(i), Cout);
                    places[j] = p;
                }
                salle.Add(places);
            }
        }

    }
}