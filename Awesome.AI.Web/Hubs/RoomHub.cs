using Awesome.AI.Common;
using Awesome.AI.Core;
using Awesome.AI.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using static Awesome.AI.Helpers.Enums;

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
        public string curr_name, curr_value, reset_name, reset_value, bcol;

        public void Setup(Instance inst)
        {
            //because robeta has UNITs sorted one way and andrew the other way
            if (inst.type == MINDS.ROBERTA)
            {
                int j = 9;
                for(int i = 0; i < 10; i++)
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
            int count_curr = 0, count_reset = 1;

            string c_name = inst.mind.stats.curr_name;
            double curr_conv = inst.mind.stats.list.Where(x=>x.name.Contains(c_name)).FirstOrDefault().conv_index;
            curr_index = "" + (int)Index(curr_conv, true, false, false);
            count_curr = inst.mind.stats.list.Where(x => x.conv_index > Index(curr_conv, false, true, false) && x.conv_index <= Index(curr_conv, false, false, true)).Sum(x => x.count_all);

            if(!inst.mind.stats.curr_name.IsNullOrEmpty())
            {
                string r_name = inst.mind.stats.curr_name;
                double reset_conv = inst.mind.stats.list.Where(x => x.name.Contains(r_name)).FirstOrDefault().conv_index;
                reset_index = "" + (int)Index(reset_conv, true, false, false);
                count_reset = inst.mind.stats.list.Where(x => x.conv_index > Index(reset_conv, false, true, false) && x.conv_index <= Index(reset_conv, false, false, true)).Sum(x => x.count_all);
            }

            curr_name = $"index below: {curr_index}0.0";
            curr_value = "" + count_curr;
            reset_name = $"index below: {reset_index}0.0";
            reset_value = "" + count_reset;
            bcol = "blue";

            if(inst.type == MINDS.ROBERTA)
            {
                curr_name = curr_name == "index below: 100.0" ? "good" : curr_name == "index below: 10.0" ? "bad" : curr_name;
                reset_name = reset_name == "index below: 10.0" ? "bad" : reset_name == "index below: 100.0" ? "good" : curr_name;
            }
            else
            {
                curr_name = curr_name == "index below: 100.0" ? "bad" : curr_name == "index below: 10.0" ? "good" : curr_name;
                reset_name = reset_name == "index below: 10.0" ? "good" : reset_name == "index below: 100.0" ? "bad" : curr_name;
            }
        }

        private string Extract(string str)
        {
            if(str.IsNullOrEmpty())
                return "";

            string res = new String(str.Where(Char.IsDigit).ToArray());

            return res;
        }

        private double Index(double index, bool is_index, bool is_lower, bool is_upper)
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
    }

    public class RoomHub : Hub
    {
        private RoomHelper helper {  get; set; }

        private bool running = false;
        
        [Authorize]
        public async Task Start()
        {
            try
            {
                if (running)
                    return;

                running = true;

                XmlHelper.WriteError("no error");
                XmlHelper.WriteMessage("starting.. 0");
                UserHelper.MaintainUsers();

                helper = new RoomHelper();

                Instance roberta = new Instance();
                Instance andrew = new Instance();

                roberta.mind = new TheMind(MECHANICS.HILL);
                andrew.mind = new TheMind(MECHANICS.CONTEST);

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
                
                XmlHelper.WriteMessage("starting.. 1");

                int when_active = 20;
                int when_inactive = 60 * 5;

                int count = 0;
                while (true)
                {
                    await Task.Delay(1000);

                    roberta.is_active = count < 10 ? true : helper.RobertaActive();
                    andrew.is_active = count < 10 ? true : !helper.RobertaActive();
                    roberta.sec_message = await helper.MessageDelay(roberta, when_active, when_inactive);
                    andrew.sec_message = await helper.MessageDelay(andrew, when_active, when_inactive);
                    count++;

                    if (count > 20)
                        count = 15;

                    XmlHelper.WriteMessage("running.. " + count);
                }

            }
            catch (Exception ex)
            {
                XmlHelper.WriteError("start - " + ex.Message);
                
                await Task.Delay(5000);

                running = false;
                Start();
            }
        }

        private long Remaining(Instance inst)
        {
            int ms_wait = inst.sec_message * 1000;
            long remainingSec = (ms_wait - inst.elapsedMs) / 1000;

            return remainingSec;
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
                    int ms_wait = inst.sec_message * 1000;
                    bool wait2 = ((double)inst.elapsedMs / (double)ms_wait) < 1.0d;
                    
                    if(wait1)
                        await Task.Delay(1000);
                    else if (wait2)
                    {
                        for (int i = 0; i <= Remaining(inst); i++)
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
                        dot = helper.GPTGiveMeADot(common);
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

                await Task.Delay(5000);

                ProcessMessage(inst);
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
                    
                    GraphInfo graph = new GraphInfo();
                    graph.Setup(inst);

                    int user_count = UserHelper.CountUsers();

                    if (user_count > 0)
                    {
                        if (inst.type == MINDS.ROBERTA)
                        {
                            await Clients.All.SendAsync("MIND1InfoReceive", momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg);
                            await Clients.All.SendAsync("MIND1GraphReceive", graph.labels, graph.curr_name, graph.curr_value, graph.reset_name, graph.reset_value, graph.bcol);
                        }

                        if (inst.type == MINDS.ANDREW)
                        {
                            await Clients.All.SendAsync("MIND2InfoReceive", momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg);
                            await Clients.All.SendAsync("MIND2GraphReceive", graph.labels, graph.curr_name, graph.curr_value, graph.reset_name, graph.reset_value, graph.bcol);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XmlHelper.WriteError("processinfo - " + ex.Message);

                await Task.Delay(5000);

                ProcessInfo(inst);
            }
        }
    }
}