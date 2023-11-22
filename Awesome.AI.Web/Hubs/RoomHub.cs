using Awesome.AI.Common;
using Awesome.AI.Core;
using Awesome.AI.Web.Helpers;
using Microsoft.AspNetCore.SignalR;
using static Awesome.AI.Helpers.Enums;

namespace Awesome.AI.Web.Hubs
{
    public class Instance
    {
        public TheMind mind { get; set; }

        public MicroLibrary.MicroTimer microTimer = new MicroLibrary.MicroTimer();
        public int sec = 1000;
        public int sec_message = 15;
        public int sec_info = 1;
        public bool fast_responce = false;
        public bool is_active = false;

        public long elapsedMs = 0;

        public MINDS type;
    }

    public class RoomHub : Hub
    {
        private RoomHelper helper {  get; set; }

        private bool running = false;
        
        public async Task Start()
        {
            try
            {
                if (running)
                    return;

                running = true;

                helper = new RoomHelper();

                Instance roberta = new Instance();
                Instance robbie = new Instance();

                roberta.mind = new TheMind(MECHANICS.HILL);
                robbie.mind = new TheMind(MECHANICS.GRAVITY);

                roberta.type = MINDS.ROBERTA;
                robbie.type = MINDS.ROBBIE;

                // Instantiate new MicroTimer and add event handler
                roberta.microTimer.MicroTimerElapsed += new MicroLibrary.MicroTimer.MicroTimerElapsedEventHandler(roberta.mind.Run);
                roberta.microTimer.Interval = roberta.mind.parms.micro_sec; // Call micro timer every 1000µs (1ms)
                roberta.microTimer.Enabled = true; // Start timer

                robbie.microTimer.MicroTimerElapsed += new MicroLibrary.MicroTimer.MicroTimerElapsedEventHandler(robbie.mind.Run);
                robbie.microTimer.Interval = robbie.mind.parms.micro_sec; // Call micro timer every 1000µs (1ms)
                robbie.microTimer.Enabled = true; // Start timer

                // Can choose to ignore event if late by Xµs (by default will try to catch up)
                //microTimer.IgnoreEventIfLateBy = 500; // 500µs (0.5ms)

                ProcessInfo(roberta);
                ProcessMessage(roberta);

                ProcessInfo(robbie);
                ProcessMessage(robbie);

                int count = 0;
                while (true)
                {
                    roberta.is_active = count < 10 ? true : helper.RobertaActive();
                    robbie.is_active = count < 10 ? true : !helper.RobertaActive();
                    roberta.sec_message = await helper.SetMessageTimer(roberta.fast_responce, roberta.is_active);
                    robbie.sec_message = await helper.SetMessageTimer(robbie.fast_responce, robbie.is_active);
                    count++;

                    if (count > 20)
                        count = 15;
                }
            }
            catch (Exception ex)
            {
                ;
                //await Clients.All.SendAsync("MessageReceive", "error in stream, please rejoin.");
            }
        }

        private bool wait1 = false;
        public async Task ProcessMessage(Instance inst)
        {
            try
            {
                await Task.Delay(100);

                List<string> dots = new List<string>() { "No emotions that is a bummer" };

                while (inst.mind.ok)
                {
                    int ms_wait = inst.sec_message * 1000;
                    long remainingSec = (ms_wait - inst.elapsedMs) / 1000;
                    bool wait2 = ((double)inst.elapsedMs / (double)ms_wait) < 1.0d;
                    
                    if(wait1)
                        await Task.Delay(1000);
                    else if (wait2)
                    {
                        for (int i = 0; i <= remainingSec; i++)
                            await Task.Delay((int)inst.sec);
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
                        subject = helper.Format1(common.HUB.subject.ToLower());
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
                        dot = helper.Format3(dot);
                        subject = helper.Format1(common.HUB.subject.ToLower());
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

                        if (inst.type == MINDS.ROBERTA)
                            await Clients.All.SendAsync("MIND1MessageReceive", message, dot1, dot2, subject);
                        if (inst.type == MINDS.ROBBIE)
                            await Clients.All.SendAsync("MIND2MessageReceive", message, dot1, dot2, subject);
                    }

                    watch.Stop();
                    inst.elapsedMs = watch.ElapsedMilliseconds;
                }
            }
            catch (Exception ex)
            {
                ;
                //await Clients.All.SendAsync("MessageReceive", "error in stream, please rejoin.");
            }
        }

        public async Task ProcessInfo(Instance inst)
        {
            try
            {
                await Task.Delay(100);
                
                while (inst.mind.ok)
                {
                    for (int i = 0; i < inst.sec_info; i++)
                        await Task.Delay((int)inst.sec);
                    
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

                    string[] labels = inst.mind.stats.list.Select(x=>x.name).ToArray();
                    string curr = inst.mind.stats.curr;
                    string value = "" + inst.mind.stats.value;
                    string curr_reset = inst.mind.stats.curr_reset;
                    string value_reset = "" + inst.mind.stats.value_reset;
                    string bcol = "blue";

                    if (inst.type == MINDS.ROBERTA) {
                        await Clients.All.SendAsync("MIND1InfoReceive", momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg);
                        await Clients.All.SendAsync("MIND1GraphReceive", labels, curr, value, curr_reset, value_reset, bcol);
                    }

                    if (inst.type == MINDS.ROBBIE) {
                        await Clients.All.SendAsync("MIND2InfoReceive", momentum, cycles, pain, position, ratio_yes, ratio_no, the_choise_isno, bias, limit, limit_avg);
                        await Clients.All.SendAsync("MIND2GraphReceive", labels, curr, value, curr_reset, value_reset, bcol);
                    }
                }
            }
            catch (Exception ex)
            {
                ;
                //await Clients.All.SendAsync("MessageReceive", "error in stream, please rejoin.");
            }
        }
    }
}