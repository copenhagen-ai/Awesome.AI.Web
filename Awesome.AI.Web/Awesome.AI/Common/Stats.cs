namespace Awesome.AI.Common
{
    public class Stat
    {
        public string name { get; set; }
        public int count_all { get; set; }
        public double force { get; set; }
        public double index_conv { get; set; }
    }

    public class Stats
    {
        public string curr_name { get; set; }
        public int curr_value { get; set; }
        public string reset_name { get; set; }
        public int reset_value { get; set; }

        public List<Stat> list = new List<Stat>();
    }

}
