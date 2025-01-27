using Org.BouncyCastle.Crypto.Modes;
using static Awesome.AI.Helpers.Enums;

namespace Awesome.AI.Web.Common
{
    public class ChatComm
    {
        public static Dictionary<MINDS, List<string>> communication { get; set; }

        public static void Add(MINDS _m, string line)
        {
            try
            {
                if (communication == null)
                {
                    communication = new Dictionary<MINDS, List<string>>();
                    communication.Add(MINDS.ROBERTA, new List<string>());
                    communication.Add(MINDS.ANDREW, new List<string>());
                }

                //if (communication[_m] == null)
                //    communication[_m] = new List<string>();

                communication[_m].Add(line);

                int count = communication[_m].Count;
                while (count >= 6) {
                    communication[_m].RemoveAt(0);
                    count = communication[_m].Count;
                }
            }
            catch(Exception _e)
            {
                throw;
            }
        }

        public static string GetResponce(MINDS _m)
        {
            try
            {
                if (communication == null)
                {
                    communication = new Dictionary<MINDS, List<string>>();
                    communication.Add(MINDS.ROBERTA, new List<string>());
                    communication.Add(MINDS.ANDREW, new List<string>());
                }

                string res = "";
                foreach (string str in ChatComm.communication[_m])
                    res += str;

                 return res;
            }
            catch(Exception _e)
            {
                throw;
            }
        }
    }
}
