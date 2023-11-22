using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awesome.AI.Common
{
    public class Stat
    {
        public string name { get; set; }
        public int count_all { get; set; }
        public double force { get; set; }
    }

    public class Stats
    {
        public string curr { get; set; }
        public int value { get; set; }
        public string curr_reset { get; set; }
        public int value_reset { get; set; }

        public List<Stat> list = new List<Stat>();
    }

}
