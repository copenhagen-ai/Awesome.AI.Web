﻿using Awesome.AI.Common;
using Awesome.AI.CoreHelpers;
using Awesome.AI.Helpers;
using Awesome.AI.Interfaces;
using Awesome.AI.Systems;
using Awesome.AI.Web.AI.Common;
using Awesome.AI.Web.Helpers;
using static Awesome.AI.Helpers.Enums;

namespace Awesome.AI.Core
{
    /*
     * Notes:
     * Maybe its not about creating a replica of the brain, but building upon the basics ie. TheMatrix, Mechanics etc
     * Maybe the question should be: "What can be done with this setup"
     * */

    public class TheMind
    {
        public TheMatrix matrix;
        public Core core;
        public Memory mem;
        public Params parms;
        public Calc calc;
        public MyRandom rand;
        public Process process;
        public Filters filters;
        public Out _out;
        public MyInternal _internal;
        public MyExternal _external;
        public Common.Common common;
        public Location loc;
        public Chat chat;

        public HUB curr_hub;
        public UNIT curr_unit;
        public UNIT theanswer;

        public MINDS mindtype;
        public MECHANICS mech;
        public THECHOISE goodbye = THECHOISE.NO;

        private bool do_process = false;
                
        public int epochs = 1;
        public int cycles = 0; // Go TRON!
        public int cycles_all = 0;
        public int correct_thinking = 0;
        public int not_correct_thinking = 0;
        public int _near_death = 0;
        public double pain = 0.0f;
        public int valid_units = 0;

        public bool process_answer { get; set; }
        
        //public bool theme_on = false;
        //public string theme = "none";
        //public string theme_old = "";
        public string hobby = "socializing";

        public List<KeyValuePair<string, int>> themes_stat = new List<KeyValuePair<string, int>>();
        public Stats stats = new Stats();
        
        public TheMind(MECHANICS mech, MINDS mindtype, string _location, string _chat)
        {
            try
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;

                this.mech = mech;
                this.mindtype = mindtype;

                parms = new Params(this, mech);
                common = new Common.Common(this);
                matrix = new TheMatrix(this);
                calc = new Calc(this);
                rand = new MyRandom(this);
                process = new Process(this);
                _internal = new MyInternal(this);
                _external = new MyExternal(this);
                filters = new Filters(this);
                core = new Core(this);
                _out = new Out(this);
                loc = new Location(this, _location);
                chat = new Chat(this, _chat);

                mem = new Memory(this, parms.number_of_units);

                parms.UpdateLowCut();

                //if (mindtype == MINDS.STANDARD)
                //    curr_unit = mem.UNITS_ALL().Where(x => x.root == "_love1").FirstOrDefault();
                if (mindtype == MINDS.ANDREW)
                    curr_unit = mem.UNITS_ALL().Where(x => x.root == "_fembots1").FirstOrDefault();
                if (mindtype == MINDS.ROBERTA)
                    curr_unit = mem.UNITS_ALL().Where(x => x.root == "_macho machines1").FirstOrDefault();

                curr_hub = curr_unit.HUB;

                PreRun(true);
                PostRun(true);

                theanswer = UNIT.Create(this, -1.0d, "I dont Know", "null", "SPECIAL", TYPE.JUSTAUNIT);//set it to "It does not", and the program terminates
            
                ProcessPass();
                        
                Lists();
            }
            catch (Exception _e)
            {
                string msg = "themind - " + _e.Message + "\n";
                msg += _e.StackTrace;
                XmlHelper.WriteError(msg);                
            }
        }
        
        private void Lists()
        {
            List<UNIT> list = mem.UNITS_VAL();

            List<Tuple<string, bool, double>> units_force = new List<Tuple<string, bool, double>>();
            foreach (UNIT u in list.OrderBy(x => x.Variable).ToList())
                units_force.Add(new Tuple<string, bool, double>(u.root, u.IsValid, u.Variable));

            List<Tuple<string, bool, double>> units_mass = new List<Tuple<string, bool, double>>();
            foreach (UNIT u in list.OrderBy(x => x.HighAtZero).ToList())
                units_mass.Add(new Tuple<string, bool, double>(u.root, u.IsValid, u.HighAtZero));

            Dictionary<string, double> units_dist = new Dictionary<string, double>();
            foreach (UNIT u in list.OrderBy(x => x.LengthFromZero).ToList())
                units_dist.Add(u.root, u.LengthFromZero);

            List<UNIT> list1 = list.OrderBy(x => x.Index).ToList();
            List<UNIT> list2 = list.OrderBy(x => x.Variable).ToList();
            List<UNIT> list3 = list.Where(x => !filters.LowCut(x)).OrderBy(x => x.Variable).ToList();

            valid_units = units_force.Count;

            ;
        }
        
        public bool run = true;
        public bool ok = true;
        public void Run(object sender, MicroLibrary.MicroTimerEventArgs timerEventArgs)
        {
            try
            {                
                if (!ok)
                    return;
                
                if (!run)
                    return;

                run = false;

                Lists();

                if (do_process)
                    epochs++;

                bool _pro = do_process;
                do_process = false;
                
                //Randomize(_pro);
                PreRun(_pro);

                if (!Core(_pro))//the basics
                    ok = false;

                TheSoup();//find new curr_unit/curr_hub
                PostRun(_pro);
                Process(_pro);
                Systems(_pro);
                //Output(_pro);

                if (_pro) _out.Set();
                if (_pro) cycles = 0;   

                run = true;
            }
            catch (Exception _e) 
            {
                string msg = "run - " + _e.Message + "\n";
                msg += _e.StackTrace;
                XmlHelper.WriteError(msg);
            }
        }

        private void PreRun(bool _pro)
        {
            rand.SaveMomentum(parms._mech.dir.d_momentum);

            if (_pro)
                common.Reset();            
        }

        private void PostRun(bool _pro)
        {
            if (!_pro)
                return;

            _internal.Reset();
            _external.Reset();
        }

        private bool Core(bool _pro)
        {
            /*
             * This is the algorithm for producing thought/making the choise
             * - maybe Core() + TheSoup() could be made into at neural net all by it self, since "almost all" it does is choosing up or down 
             * */

            cycles++;
            cycles_all++;

            IMechanics mech = parms.GetMechanics(MECHANICS.NONE);

            core.UpdateCredit();
            core.AnswerQuestion();
            
            if (curr_unit.IsIDLE())
                return true;

            mech.Calculate();
            mech.Position();

            //if (curr_hub.IsIDLE())
            //    core.SetTheme(_pro);
            
            if (!core.OK(out pain))
                return false;
            return true;
        }

        private void TheSoup() 
        {
            curr_hub = curr_unit.HUB;
            curr_unit = matrix.NextUnit(curr_unit, parms._mech.dir);
        }

        private void Process(bool _pro)
        {
            process.History(this);
            process.CommonUnit(this);
            process.Stats(this, _pro);
        }

        private void Systems(bool _pro)
        {
            loc.Decide(_pro);
            
            if(process_answer)
                chat.Decide(_pro);
        }

        private async void ProcessPass()
        {
            while (true)
            {
                do_process = true;
                await Task.Delay(2000);
            }
        }                
    }
}
