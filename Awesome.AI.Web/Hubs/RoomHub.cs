using Awesome.AI.Common;
using Awesome.AI.Core;
using Awesome.AI.Web.Helpers;
using Microsoft.AspNetCore.SignalR;
using static Awesome.AI.Helpers.Enums;
using static Awesome.AI.Web.Common.Enums;

namespace Awesome.AI.Web.Hubs
{
    public class Instance
    {
        public TheMind mind { get; set; }

        public MicroLibrary.MicroTimer microTimer = new MicroLibrary.MicroTimer();
        //public int sec = 1000;
        public int sec_message = 1;//not set here
        public int sec_info = 1;
        public bool fast_responce = false;
        public bool is_active = false;

        public long elapsedMs = 0;

        public MINDS type;
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
            int count_curr = 0, count_reset = 0;

            string c_name = inst.mind.stats.curr_name;
            double curr_conv = inst.mind.stats.list.Where(x => x.name.Contains(c_name)).FirstOrDefault().index_conv;
            curr_index = "" + (int)FormatIndex(curr_conv, true, false, false);
            count_curr = inst.mind.stats.list.Where(x => x.index_conv > FormatIndex(curr_conv, false, true, false) && x.index_conv <= FormatIndex(curr_conv, false, false, true)).Sum(x => x.count_all);

            curr_name = $"index below: {curr_index}0.0";
            curr_value = "" + count_curr;

            if (!inst.mind.stats.reset_name.IsNullOrEmpty())
            {
                string r_name = inst.mind.stats.reset_name;
                double reset_conv = inst.mind.stats.list.Where(x => x.name.Contains(r_name)).FirstOrDefault().index_conv;
                reset_index = "" + (int)FormatIndex(reset_conv, true, false, false);
                count_reset = inst.mind.stats.list.Where(x => x.index_conv > FormatIndex(reset_conv, false, true, false) && x.index_conv <= FormatIndex(reset_conv, false, false, true)).Sum(x => x.count_all);

                reset_name = $"index below: {reset_index}0.0";
                reset_value = "" + count_reset;
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
            int count_curr = 0, count_reset = 0;

            string c_name = inst.mind.stats.curr_name;
            double curr_force = inst.mind.stats.list.Where(x => x.name.Contains(c_name)).FirstOrDefault().force;
            curr_index = "" + (int)FormatForce(inst.mind, curr_force, true, false, false);
            count_curr = inst.mind.stats.list.Where(x => x.force > FormatForce(inst.mind, curr_force, false, true, false) && x.force <= FormatForce(inst.mind, curr_force, false, false, true)).Sum(x => x.count_all);

            curr_name = $"force: {curr_index}0.0";
            curr_value = "" + count_curr;

            if (!inst.mind.stats.reset_name.IsNullOrEmpty())
            {
                string r_name = inst.mind.stats.reset_name;
                double reset_force = inst.mind.stats.list.Where(x => x.name.Contains(r_name)).FirstOrDefault().force;
                reset_index = "" + (int)FormatForce(inst.mind, reset_force, true, false, false);
                count_reset = inst.mind.stats.list.Where(x => x.force > FormatForce(inst.mind, reset_force, false, true, false) && x.force <= FormatForce(inst.mind, reset_force, false, false, true)).Sum(x => x.count_all);

                reset_name = $"force: {reset_index}0.0";
                reset_value = "" + count_reset;
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
        private RoomHelper helper {  get; set; }
        private RUNNING running { get; set; }
        //private GRAPH is_graph = GRAPH.UNIT;

        private static bool is_running = false;


        //[Authorize]
        public async Task Start()
        {
            try
            {
                if (is_running)
                    return;

                is_running = true;

                XmlHelper.ClearError("no error");
                XmlHelper.WriteMessage("starting.. 0");
                UserHelper.MaintainUsers();

                helper = new RoomHelper();

                running = RUNNING.BOTH;

                Instance roberta = new Instance();
                Instance andrew = new Instance();

                if(running == RUNNING.BOTH)
                {
                    roberta.mind = new TheMind(MECHANICS.HILL, "roberta");
                    andrew.mind = new TheMind(MECHANICS.CONTEST, "andrew");

                    roberta.type = MINDS.ROBERTA;
                    andrew.type = MINDS.ANDREW;

                    // Instantiate new MicroTimer and add event handler
                    roberta.microTimer.MicroTimerElapsed += new MicroLibrary.MicroTimer.MicroTimerElapsedEventHandler(roberta.mind.Run);
                    roberta.microTimer.Interval = roberta.mind.parms.micro_sec; // Call micro timer every 1000µs (1ms)
                    roberta.microTimer.Enabled = true; // Start timer

                    andrew.microTimer.MicroTimerElapsed += new MicroLibrary.MicroTimer.MicroTimerElapsedEventHandler(andrew.mind.Run);
                    andrew.microTimer.Interval = andrew.mind.parms.micro_sec; // Call micro timer every 1000µs (1ms)
                    andrew.microTimer.Enabled = true; // Start timer

                    // Can choose to ignore event if late by Xµs (by default will try to catch up)
                    //microTimer.IgnoreEventIfLateBy = 500; // 500µs (0.5ms)

                    ProcessInfo(roberta);
                    ProcessMessage(roberta);

                    ProcessInfo(andrew);
                    ProcessMessage(andrew);
                }
                else if (running == RUNNING.ROBERTA)
                {
                    roberta.mind = new TheMind(MECHANICS.HILL, "roberta");
                    roberta.type = MINDS.ROBERTA;

                    // Instantiate new MicroTimer and add event handler
                    roberta.microTimer.MicroTimerElapsed += new MicroLibrary.MicroTimer.MicroTimerElapsedEventHandler(roberta.mind.Run);
                    roberta.microTimer.Interval = roberta.mind.parms.micro_sec; // Call micro timer every 1000µs (1ms)
                    roberta.microTimer.Enabled = true; // Start timer

                    // Can choose to ignore event if late by Xµs (by default will try to catch up)
                    //microTimer.IgnoreEventIfLateBy = 500; // 500µs (0.5ms)

                    ProcessInfo(roberta);
                    ProcessMessage(roberta);

                    andrew = null;
                }
                else if(running == RUNNING.ANDREW)
                {
                    //andrew.mind = new TheMind(MECHANICS.CONTEST, "andrew");
                    andrew.mind = new TheMind(MECHANICS.CONTEST, "standart remember");
                    andrew.type = MINDS.ANDREW;
                    
                    andrew.microTimer.MicroTimerElapsed += new MicroLibrary.MicroTimer.MicroTimerElapsedEventHandler(andrew.mind.Run);
                    andrew.microTimer.Interval = andrew.mind.parms.micro_sec; // Call micro timer every 1000µs (1ms)
                    andrew.microTimer.Enabled = true; // Start timer

                    ProcessInfo(andrew);
                    ProcessMessage(andrew);

                    roberta = null;
                }


                
                XmlHelper.WriteMessage("starting.. 1");

                int when_active = 20;
                int when_inactive = 60 * 5;

                int count = 0;
                while (true)
                {
                    if (!is_running)
                        throw new Exception("not is_running");

                    await Task.Delay(1000);

                    if (running == RUNNING.BOTH)
                    {
                        roberta.is_active = count < 10 ? true : helper.RobertaActive();
                        andrew.is_active = count < 10 ? true : !helper.RobertaActive();
                        roberta.sec_message = await helper.MessageDelay(roberta, when_active, when_inactive);
                        andrew.sec_message = await helper.MessageDelay(andrew, when_active, when_inactive);
                    }
                    else if (running == RUNNING.ROBERTA)
                    {
                        roberta.is_active = count < 10 ? true : true;
                        //andrew.is_active = count < 10 ? true : !helper.RobertaActive();
                        roberta.sec_message = 20;
                        //andrew.sec_message = await helper.MessageDelay(andrew, when_active, when_inactive);
                    }
                    if (running == RUNNING.ANDREW)
                    {
                        //roberta.is_active = count < 10 ? true : helper.RobertaActive();
                        andrew.is_active = count < 10 ? true : true;
                        //roberta.sec_message = await helper.MessageDelay(roberta, when_active, when_inactive);
                        andrew.sec_message = 20;
                    }

                    count++;

                    if (count > 20)
                        count = 15;

                    XmlHelper.WriteMessage("running.. " + count);
                }

            }
            catch (Exception ex)
            {
                XmlHelper.WriteError("start - " + ex.Message);
                
                //await Task.Delay(5000);

                is_running = false;
                //Start();
            }
        }

        private bool wait1 = false;
        private async Task ProcessMessage(Instance inst)
        {
            try
            {
                await Task.Delay(100);

                List<string> dots = new List<string>() { "No emotions that is a bummer" };

                while (inst.mind.ok)
                {
                    if (!is_running)
                        throw new Exception("not is_running");

                    int ms_wait = inst.sec_message * 1000;
                    bool wait2 = ((double)inst.elapsedMs / (double)ms_wait) < 1.0d;
                    
                    if(wait1)
                        await Task.Delay(1000);
                    else if (wait2)
                    {
                        for (int i = 0; i <= helper.Remaining(inst); i++)
                            await Task.Delay(1000);
                    }
                                        
                    wait1 = false;

                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    //inst.elapsedMs = 0;
                    // the code that you want to measure comes here

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
                            inst.elapsedMs = 0;
                            wait1 = true;
                            continue;
                        }
                        dot = helper.Format1(dot);
                        subject = common.HUB.subject.ToLower();
                    }

                    dots.Add(dot);
                    
                    while(dots.Count > 2)
                        dots.RemoveAt(0);

                    if(dots.Count > 1)
                    {
                        string dot1 = dots[0];
                        string dot2 = dots[1];

                        string message = helper.GPTConnectTheDots(dot1, dot2, ref inst.fast_responce);
                        
                        message = message.Replace(dot1, $"<span class=\"i-color-green\">{dot1}</span>");
                        message = message.Replace(dot2, $"<span class=\"i-color-red\">{dot2}</span>");

                        int user_count = UserHelper.CountUsers();

                        if(user_count > 0)
                        {
                            if (inst.type == MINDS.ROBERTA)
                                await Clients.All.SendAsync("MIND1MessageReceive", message, dot1, dot2, subject);
                            if (inst.type == MINDS.ANDREW)
                                await Clients.All.SendAsync("MIND2MessageReceive", message, dot1, dot2, subject);
                        }
                    }

                    watch.Stop();
                    inst.elapsedMs = watch.ElapsedMilliseconds;
                }
            }
            catch (Exception ex)
            {
                XmlHelper.WriteError("processmessage - " + ex.Message);

                //await Task.Delay(5000);

                //ProcessMessage(inst);

                is_running = false;
            }
        }

        private async Task ProcessInfo(Instance inst)
        {
            try
            {
                await Task.Delay(100);
                
                while (inst.mind.ok)
                {
                    if (!is_running)
                        throw new Exception("not is_running");

                    for (int i = 0; i < inst.sec_info; i++)
                        await Task.Delay(1000);
                    
                    string[] cycles = new string[] { inst.mind._out.cycles, inst.mind._out.cycles_total };
                    string momentum = inst.mind._out.momentum;

                    string pain = inst.mind._out.pain;
                    string position = inst.mind._out.position;
                    string ratio_yes = inst.mind._out.ratio_yes;
                    string ratio_no = inst.mind._out.ratio_no;
                    string the_choise_isno = inst.mind._out.the_choise_isno;

                    string bias = inst.mind._out.bias;
                    string limit = inst.mind._out.limit;
                    string limit_avg = inst.mind._out.limit_avg;

                    GraphInfo graph1 = new GraphInfo();
                    GraphInfo graph2 = new GraphInfo();
                    
                    //if (is_graph == GRAPH.INDEX)
                    //    graph1.SetupIndex(inst);
                    //else if (is_graph == GRAPH.FORCE)
                    //    graph1.SetupForce(inst);
                    //else
                    //    graph1.SetupUnit(inst);

                    graph1.SetupIndex(inst);
                    graph2.SetupUnit(inst);

                    int user_count = UserHelper.CountUsers();

                    if (user_count > 0)
                    {
                        if (inst.type == MINDS.ROBERTA)
                        {
                            await Clients.All.SendAsync("MIND1InfoReceive", momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg);
                            await Clients.All.SendAsync("MIND1GraphReceive", graph1.labels, graph1.curr_name, graph1.curr_value, graph1.reset_name, graph1.reset_value, graph1.bcol);
                            await Clients.All.SendAsync("MIND3GraphReceive", graph2.labels, graph2.curr_name, graph2.curr_value, graph2.reset_name, graph2.reset_value, graph2.bcol);
                        }

                        if (inst.type == MINDS.ANDREW)
                        {
                            await Clients.All.SendAsync("MIND2InfoReceive", momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg);
                            await Clients.All.SendAsync("MIND2GraphReceive", graph1.labels, graph1.curr_name, graph1.curr_value, graph1.reset_name, graph1.reset_value, graph1.bcol);
                            await Clients.All.SendAsync("MIND4GraphReceive", graph2.labels, graph2.curr_name, graph2.curr_value, graph2.reset_name, graph2.reset_value, graph2.bcol);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XmlHelper.WriteError("processinfo - " + ex.Message);

                //await Task.Delay(5000);

                if(is_running)
                    ProcessInfo(inst);
                else
                    is_running = false;
            }
        }
    }
}