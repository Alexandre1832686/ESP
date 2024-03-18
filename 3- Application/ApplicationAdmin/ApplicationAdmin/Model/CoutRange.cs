using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationAdmin.Model
{
    public class CoutRange
    {
        public string Range { get; set; }
        public double Cout { get; set; }

        public CoutRange() { }

        public CoutRange(string range,double cout)
        { 
            Range = range; 
            Cout = cout; 
        }
    }
}
