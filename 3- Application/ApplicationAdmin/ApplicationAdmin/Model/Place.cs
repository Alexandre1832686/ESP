using ApplicationAdmin.Model_View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.Model
{
    public class Place
    {
        public int Numero { get; set; }
        public string Range { get; set; }
        public bool Occupe { get; set; }
        public double Prix { get; set; }

        public Place(int numero, string range, string coutstring)
        {
            Numero = numero;
            Range = range;
            Occupe = false;

            string[] stringrangecout = coutstring.Split(',');
            int id = AppVM.ConvertRangeToInd(Range);
            Prix = Convert.ToDouble(stringrangecout[id].Replace('.', ','));

        }
    }
}

