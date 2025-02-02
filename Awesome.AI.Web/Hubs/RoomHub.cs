using Awesome.AI.Common;
using Awesome.AI.Core;
using Awesome.AI.Web.Common;
using Awesome.AI.Web.Helpers;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using static Awesome.AI.Helpers.Enums;

namespace Awesome.AI.Web.Hubs
{
    public class Bot
    {
        public MECHANICS mech {  get; set; }
        public MINDS mindtype { get; set; }

        public string location { get; set; }
    }

    public class Instance
    {
        public MicroLibrary.MicroTimer microTimer = new MicroLibrary.MicroTimer();
        public TheMind mind { get; set; }
        public MINDS type { get; set; }

        public int sec_message = 20;//not set here
        public int sec_info = 1;
        public int sec_chat = 1;
        //public bool fast_responce = false;
        //public bool is_active = false;
        //public long elapsedMs = 0;
    }

    public class GraphInfo
    {
        public string[] labels = new string[10];
        public string curr_name = "", curr_value, reset_name = "", reset_value, bcol;

        public void SetupIndex(Instance inst)
        {
            //because robeta has UNITs sorted one way and andrew the other way
            if (inst.type == MINDS.ROBERTA)
            {
                int j = 9;
                for (int i = 0; i < 10; i++)
                    labels[i] = $"index below: {(j-- + 1)}0.0";

                labels[0] = "good";
                labels[9] = "bad";
            }
            else
            {
                for (int i = 0; i < 10; i++)
                    labels[i] = $"index below: {(i + 1)}0.0";

                labels[0] = "good";
                labels[9] = "bad";
            }

            string curr_index = "", reset_index = "";
            
            string c_name = "" + inst.mind.stats.curr_name;
            Stat curr = inst.mind.stats.list.Where(x => x.name == c_name).FirstOrDefault();

            if (curr.IsNull())
                return;

            curr_index = "" + (int)FormatIndex(curr.index, true, false, false);
            
            curr_name = $"index below: {curr_index}0.0";
            curr_value = "" + inst.mind.stats.curr_value;

            if (!inst.mind.stats.reset_name.IsNullOrEmpty())
            {
                string r_name = "" + inst.mind.stats.reset_name;
                Stat reset = inst.mind.stats.list.Where(x => x.name == r_name).FirstOrDefault();

                if (reset.IsNull())
                    return;

                reset_index = "" + (int)FormatIndex(reset.index, true, false, false);
                
                reset_name = $"index below: {reset_index}0.0";
                reset_value = "" + inst.mind.stats.reset_value;
            }

            bcol = "blue";

            if (inst.type == MINDS.ROBERTA)
            {
                curr_name = curr_name == "index below: 100.0" ? "good" : curr_name == "index below: 10.0" ? "bad" : curr_name;
                reset_name = reset_name == "index below: 10.0" ? "bad" : reset_name == "index below: 100.0" ? "good" : reset_name;
            }
            else
            {
                curr_name = curr_name == "index below: 100.0" ? "bad" : curr_name == "index below: 10.0" ? "good" : curr_name;
                reset_name = reset_name == "index below: 10.0" ? "good" : reset_name == "index below: 100.0" ? "bad" : reset_name;
            }
        }

        public void SetupForce(Instance inst)
        {
            //because robeta has UNITs sorted one way and andrew the other way
            if (inst.type == MINDS.ROBERTA)
            {
                int j = 9;
                for (int i = 0; i < 10; i++)
                    labels[i] = $"force: {(j-- + 1)}0.0";

                labels[0] = "light";
                labels[9] = "heavy";
            }
            else
            {
                for (int i = 0; i < 10; i++)
                    labels[i] = $"force: {(i + 1)}0.0";

                labels[0] = "heavy";
                labels[9] = "light";
            }


            string curr_index = "", reset_index = "";
            int curr_hits = 0, reset_hits = 0;

            string c_name = "" + inst.mind.stats.curr_name;
            Stat curr = inst.mind.stats.list.Where(x => x.name.Contains(c_name)).FirstOrDefault();

            if (curr.IsNull())
                return;

            curr_index = "" + (int)FormatForce(inst.mind, curr.force, true, false, false);
            
            curr_name = $"force: {curr_index}0.0";
            curr_value = "" + inst.mind.stats.curr_value;

            if (!inst.mind.stats.reset_name.IsNullOrEmpty())
            {
                string r_name = "" + inst.mind.stats.reset_name;
                Stat reset = inst.mind.stats.list.Where(x => x.name.Contains(r_name)).FirstOrDefault();

                if (reset.IsNull())
                    return;

                reset_index = "" + (int)FormatForce(inst.mind, reset.force, true, false, false);
                
                reset_name = $"force: {reset_index}0.0";
                reset_value = "" + inst.mind.stats.reset_value;
            }

            bcol = "blue";

            if (inst.type == MINDS.ROBERTA)
            {
                curr_name = curr_name == "force: 100.0" ? "light" : curr_name == "force: 10.0" ? "heavy" : curr_name;
                reset_name = reset_name == "force: 10.0" ? "heavy" : reset_name == "force: 100.0" ? "light" : reset_name;
            }
            else
            {
                curr_name = curr_name == "force: 100.0" ? "light" : curr_name == "force: 10.0" ? "heavy" : curr_name;
                reset_name = reset_name == "force: 10.0" ? "heavy" : reset_name == "force: 100.0" ? "light" : reset_name;
            }
        }

        public void SetupUnit(Instance inst)
        {
            labels = new string[inst.mind.stats.list.Count];

            int i = 0;
            foreach (Stat stat in inst.mind.stats.list)
            {
                labels[i] = stat.name;
                i++;
            }
            
            curr_name = inst.mind.stats.curr_name;
            curr_value = "" + inst.mind.stats.curr_value;

            if (!inst.mind.stats.reset_name.IsNullOrEmpty())
            {
                reset_name = inst.mind.stats.reset_name;
                reset_value = "" + inst.mind.stats.reset_value;
            }

            bcol = "blue";
        }

        private string Extract(string str)
        {
            if(str.IsNullOrEmpty())
                return "";

            string res = new String(str.Where(Char.IsDigit).ToArray());

            return res;
        }

        private double FormatIndex(double index, bool is_index, bool is_lower, bool is_upper)
        {
            double res_index = ((int)Math.Floor(index / 10.0)) * 10 + 10.0d;

            if (is_index)
                return res_index / 10.0d;
            if (is_lower)
                return res_index - 10.0d;
            if (is_upper)
                return res_index;
            
            throw new Exception();
        }        

        private double FormatForce(TheMind mind, double index, bool is_index, bool is_lower, bool is_upper)
        {
            double min = mind.common.LowestForce().Variable;
            double max = mind.common.HighestForce().Variable;
            double temp = mind.calc.NormalizeRange(index, min, max, 0.01d, 99.99d);

            double res_index = ((int)Math.Floor(temp / 10.0)) * 10 + 10.0d;

            if (is_index)
                return res_index / 10.0d;
            if (is_lower)
                return mind.calc.NormalizeRange(res_index - 10.0d, 0.0d, 100.0d, min, max);
            if (is_upper)
                return mind.calc.NormalizeRange(res_index, 0.0d, 100.0d, min, max);

            throw new Exception();
        }
    }

    public class RoomHub : Hub
    {
        public static List<Instance> Instances = new List<Instance>();
        public static List<Bot> Bots = new List<Bot>();

        private RoomHelper helper {  get; set; }
        
        private static bool is_running = false;

        //private const int WHEN_ACTIVE = 20;
        //private const int WHEN_INACTIVE = 60 * 5;

        public static void ResetAsked()
        {
            foreach(Instance i in Instances)
                i.mind.chat_asked = false;
        }

        public static bool IsDebug()
        {
            if (Bots == null)
                throw new Exception();

            if (!Bots.Any())
                throw new Exception();

            Bot bot = Bots.First();
            MINDS type = bot.mindtype;
            bool ok = Bots.Any(x => x.mindtype != type);

            return ok;
        }
        
        public async Task Start()
        {
            try
            {
                if (is_running)
                    return;

                is_running = true;

                Instances = new List<Instance>();
                Bots = new List<Bot>();

                XmlHelper.ClearError("no error");
                XmlHelper.WriteMessage("starting.. 0");
                UserHelper.MaintainUsers();

                helper = new RoomHelper();

                int MAX = Enum.GetNames(typeof(MINDS)).Length;
                                
                Bots.Add(new Bot() { mindtype = MINDS.ROBERTA, mech = MECHANICS.HILL, location= "KITCHEN" });
                Bots.Add(new Bot() { mindtype = MINDS.ANDREW, mech = MECHANICS.CONTEST, location = "LIVINGROOM" });
                
                foreach (Bot bot in Bots)
                {
                    Instance instance = new Instance();

                    instance.mind = new TheMind(bot.mech, bot.mindtype, bot.location);
                    instance.type = bot.mindtype;
                        
                    // Instantiate new MicroTimer and add event handler
                    instance.microTimer.MicroTimerElapsed += new MicroLibrary.MicroTimer.MicroTimerElapsedEventHandler(instance.mind.Run);
                    instance.microTimer.Interval = instance.mind.parms.micro_sec; // Call micro timer every 1000µs (1ms)
                    instance.microTimer.Enabled = true; // Start timer

                    // Can choose to ignore event if late by Xµs (by default will try to catch up)
                    //microTimer.IgnoreEventIfLateBy = 500; // 500µs (0.5ms)

                    while(instance.mind.cycles_all < 10)
                        await Task.Delay(1000);

                    ProcessInfo(instance);
                    ProcessMonologue(instance);
                    ProcessChat(instance);

                    Instances.Add(instance);
                }

                XmlHelper.WriteMessage("starting.. 1");

                int counter = 0;
                while (is_running)
                {
                    await Task.Delay(1000);

                    foreach (Instance inst in Instances)
                    {
                        if (!inst.mind.ok)
                            is_running = false;
                        
                        //int index = Instances.IndexOf(inst);
                        //bool is_even = index % 2 == 0;
                        //bool is_all = Instances.Count == MAX;

                        //inst.is_active = helper.Active(is_even, is_all);
                        //inst.sec_message = helper.Delay(inst, WHEN_ACTIVE, WHEN_INACTIVE);
                    }

                    counter++;

                    if (counter > 20)
                        counter = 10;

                    XmlHelper.WriteMessage("running.. " + counter);
                    Debug.WriteLine("running.. " + counter);
                }
            }
            catch (Exception ex)
            {
                XmlHelper.WriteError("start - " + ex.Message);
                
                is_running = false;
            }
        }

        private async Task ProcessMonologue(Instance inst)
        {
            try
            {
                await Task.Delay(100);

                List<string> dots = new List<string>() { "feeling great" };
                bool wait1 = false;

                while (inst.mind.ok)
                {
                    //int ms_wait = inst.sec_message * 1000;
                    //bool wait2 = ((double)inst.elapsedMs / (double)ms_wait) < 1.0d;

                    //if(wait1)
                    //    await Task.Delay(1000);
                    //else if (wait2)
                    //{
                    //    for (int i = 0; i <= helper.Remaining(inst, is_running); i++)
                    //        await Task.Delay(1000);
                    //}

                    //wait1 = false;

                    if (wait1) {
                        await Task.Delay(1000);
                        wait1 = false;
                    }
                    else {
                        for (int i = 0; i <= inst.sec_message; i++)
                            await Task.Delay(1000);
                    }


                    if (!is_running)
                        throw new Exception("not is_running");

                    //var watch = System.Diagnostics.Stopwatch.StartNew();

                    // the code that you want to measure comes here

                    int user_count = UserHelper.CountUsers();

                    if (user_count > 1 || IsDebug())
                    {

                        string subject = "";
                        string dot = "";
                        if (inst.mind.parms.matrix_type == MATRIX.SIMPLE)
                        {
                            UNIT common = inst.mind._out.common_unit;
                            dot = helper.Format1(common.root.ToLower());
                            subject = common.HUB.subject.ToLower();
                        }
                        else
                        {
                            UNIT common = inst.mind._out.common_unit;
                            dot = helper.GPTGiveMeADot(inst, common);

                            if (dot.IsNullOrEmpty())
                            {
                                //inst.elapsedMs = 0;
                                wait1 = true;
                                continue;
                            }

                            dot = helper.Format1(dot);
                            subject = common.HUB.subject.ToLower();
                        }

                        dots.Add(dot);

                        while (dots.Count > 2)
                            dots.RemoveAt(0);

                        if (dots.Count > 1)
                        {
                            string dot1 = dots[0];
                            string dot2 = dots[1];

                            string message = helper.GPTConnectTheDots(dot1, dot2/*, ref inst.fast_responce*/);

                            message = message.Replace(dot1, $"<span class=\"i-color-green\">{dot1}</span>");
                            message = message.Replace(dot2, $"<span class=\"i-color-orange\">{dot2}</span>");

                            if (inst.type == MINDS.ROBERTA)
                                await Clients.All.SendAsync("MIND1MessageReceive", message, dot1, dot2, subject);
                            if (inst.type == MINDS.ANDREW)
                                await Clients.All.SendAsync("MIND2MessageReceive", message, dot1, dot2, subject);
                        }
                        
                        //watch.Stop();
                        //inst.elapsedMs = watch.ElapsedMilliseconds;
                    }
                }
            }
            catch (Exception ex)
            {
                //if (inst.mind.ok)
                //    ProcessMessage(inst);
                //else
                {
                    XmlHelper.WriteError("processmessage - " + ex.Message);

                    //inst.mind.theanswer.root = "It does not";

                    inst.microTimer.Enabled = false;

                    is_running = false;
                }
            }
        }

        private async Task ProcessInfo(Instance inst)
        {
            try
            {
                await Task.Delay(100);

                while (inst.mind.ok)
                {
                    for (int i = 0; i < inst.sec_info; i++)
                        await Task.Delay(1000);

                    if (!is_running)
                        throw new Exception("not is_running");

                    string[] cycles = new string[] { inst.mind._out.cycles, inst.mind._out.cycles_total };
                    string momentum = ("" + inst.mind._out.momentum).Length < 5 ? inst.mind._out.momentum : $"{inst.mind._out.momentum}"[..10];

                    string pain = ("" + inst.mind._out.pain).Length < 5 ? inst.mind._out.pain : $"{inst.mind._out.pain}"[..5];
                    string position = ("" + inst.mind._out.position).Length < 5 ? inst.mind._out.position : $"{inst.mind._out.position}"[..5];
                    string[] ratio = new string[] { "" + inst.mind._out.ratio_yes, "" + inst.mind._out.ratio_no };
                    string the_choise = inst.mind._out.the_choise;

                    string epochs = inst.mind._out.epochs;
                    string runtime = inst.mind._out.runtime;
                    string occu = inst.mind._out.occu;
                    string locationfinal = inst.mind._out.location;
                    string loc_state = inst.mind._out.loc_state;
                    string chat_state = inst.mind._out.chat_state;

                    GraphInfo graph1 = new GraphInfo();
                    GraphInfo graph2 = new GraphInfo();

                    graph1.SetupIndex(inst);
                    graph2.SetupUnit(inst);

                    int user_count = UserHelper.CountUsers();

                    if (user_count > 1 || IsDebug())
                    {
                        if (inst.type == MINDS.ROBERTA)
                        {
                            await Clients.All.SendAsync("MIND1InfoReceive1", epochs, runtime, momentum, cycles, pain, position, ratio, the_choise);
                            await Clients.All.SendAsync("MIND1InfoReceive2", occu, locationfinal, loc_state, chat_state);
                            await Clients.All.SendAsync("MIND1GraphReceive", graph1.labels, graph1.curr_name, graph1.curr_value, graph1.reset_name, graph1.reset_value, graph1.bcol);
                            await Clients.All.SendAsync("MIND3GraphReceive", graph2.labels, graph2.curr_name, graph2.curr_value, graph2.reset_name, graph2.reset_value, graph2.bcol);
                        }

                        if (inst.type == MINDS.ANDREW)
                        {
                            await Clients.All.SendAsync("MIND2InfoReceive1", epochs, runtime, momentum, cycles, pain, position, ratio, the_choise);
                            await Clients.All.SendAsync("MIND2InfoReceive2", occu, locationfinal, loc_state, chat_state);
                            await Clients.All.SendAsync("MIND2GraphReceive", graph1.labels, graph1.curr_name, graph1.curr_value, graph1.reset_name, graph1.reset_value, graph1.bcol);
                            await Clients.All.SendAsync("MIND4GraphReceive", graph2.labels, graph2.curr_name, graph2.curr_value, graph2.reset_name, graph2.reset_value, graph2.bcol);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //if (inst.mind.ok)
                //    ProcessInfo(inst);
                //else
                {
                    XmlHelper.WriteError("processinfo - " + ex.Message);

                    //inst.mind.theanswer.root = "It does not";

                    inst.microTimer.Enabled = false;

                    is_running = false;
                }
            }
        }

        private async Task ProcessChat(Instance inst)
        {
            try
            {
                await Task.Delay(100);

                while (inst.mind.ok)
                {
                    for (int i = 0; i < inst.sec_chat; i++)
                        await Task.Delay(1000);

                    if (!is_running)
                        throw new Exception("not is_running");

                    if (inst.mind._out.chat_subject == "")
                        continue;

                    //if (inst.mind.chatans.ChatState > 0)
                    //    continue;

                    string subject = inst.mind._out.chat_subject;
                    inst.mind._out.chat_subject = "";

                    int user_count = UserHelper.CountUsers();

                    if (user_count > 1 || IsDebug())
                    {
                        string ask = helper.GPTAskMeAQuestion(inst, subject);

                        if (ask.IsNullOrEmpty())
                            ask = ". . .";

                        ChatComm.Add(inst.type, $">> ass:{ask}<br>");

                        if (inst.type == MINDS.ROBERTA) {
                            string res = ChatComm.GetResponce(inst.type);
                            await Clients.All.SendAsync("MIND1ChatReceive1", res);
                        }

                        if (inst.type == MINDS.ANDREW) {
                            string res = ChatComm.GetResponce(inst.type);
                            await Clients.All.SendAsync("MIND2ChatReceive1", res);
                        }

                        inst.mind.chat_asked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //if (inst.mind.ok)
                //    ProcessInfo(inst);
                //else
                {
                    XmlHelper.WriteError("processinfo - " + ex.Message);

                    //inst.mind.theanswer.root = "It does not";

                    inst.microTimer.Enabled = false;

                    is_running = false;
                }
            }
        }
    }
}