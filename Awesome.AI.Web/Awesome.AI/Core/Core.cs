﻿using Awesome.AI.Common;
using Awesome.AI.Helpers;
using static Awesome.AI.Helpers.Enums;

namespace Awesome.AI.Core
{
    public class Core
    {
        private TheMind mind;
        private Core() { }
        public Core(TheMind mind)
        {
            this.mind = mind;
        }

        public bool OK(out double pain)
        {
            /*
             * this is the Go/NoGo class
             * actually not part of the algorithm
             * */
            try
            {
                double _e = Constants.position == POSITION.OLD ?
                    mind.parms._mech.POS_XY :
                    mind.pos.Pos;

                pain = mind.calc.Reciprocal(_e);

                if (pain > 10.0)
                    throw new Exception();

                return pain < mind.parms.max_pain;
            }
            catch (Exception e)//thats it
            {
                pain = mind.parms.max_pain;
                return false;
            }
        }

        public void AnswerQuestion()
        {
            /*
             * ..or if all goals are fulfilled?
             * ..or if can make consious choise
             * should there be some procedure for this(unlocking)?
             * */

            for (int i = 0; i <= 10; i++)
            {
                if ((mind.epochs - i) == (60 * mind.parms.runtime))
                    mind.theanswer.root = "It does not";
            }
            
            string answer = mind.theanswer.root;
            
            if (answer == null)
                throw new ArgumentNullException();

            mind.goodbye = answer == "It does not" ? THECHOISE.YES : THECHOISE.NO;
        }

        public void UpdateCredit()
        {
            //List<UNIT> list = mind.mem.UNITS_VAL();
            List<UNIT> list = mind.mem.UNITS_ALL();

            //this could be a problem with many hubs
            foreach (UNIT _u in list)
            {
                if (_u.root == mind.curr_unit.root)
                    continue;

                double cred = mind.parms.update_cred;
                _u.credits += cred;

                if (_u.credits > Constants.MAX_CREDIT)
                    _u.credits = Constants.MAX_CREDIT;
            }

            mind.curr_unit.credits -= 1.0d;
            if (mind.curr_unit.credits < 0.0d)
                mind.curr_unit.credits = 0.0d;
        }
        
        

        //private bool passed = false;
        //private bool IsConsious()//something like this :)
        //{
        //    /*
        //     * This is VERY experimental
        //     * 
        //     * maybe consiousness isnt a constant.. if you pass "this" test, then in the moment you are consious
        //     * 
        //     * this should be dependant on things like:
        //     * tiredness/stamina
        //     * outside pressure
        //     * 
        //     * question: do you like this topic, then follow 
        //     * */
        //    if (mind.process.StreamTop().HUB.IsLEARNING())
        //        return false;
        //    if (mind.curr_hub.IsLEARNING())
        //        return false;

        //    if (PassTest())
        //        return true;
            
        //    if(passed)
        //        return mind.process.StreamTop().HUB.GetSubject() == mind.theme;
            
        //    return false;
        //}

        //private bool PassTest()
        //{
        //    if (!passed && mind.calc.Chance0to1(0.95d, true))
        //    {
        //        passed = true;
        //        return true;
        //    }
        //    return false;
        //}

        //public void SetTheme(bool _pro) 
        //{
        //    //update theme

        //    if (!_pro)
        //        return;

        //    mind.theme = IsConsious() ? mind.curr_hub.GetSubject() : mind.theme;
        //    if (mind.theme_old != mind.theme)
        //    {
        //        mind.theme_old = mind.theme;
        //        mind.themes_stat.Add(new KeyValuePair<string, int>(mind.theme, 0));
        //        if (mind.themes_stat.Count > 10)
        //            mind.themes_stat.RemoveAt(0);
        //    }
        //    if(mind.themes_stat.Count > 0)
        //        mind.themes_stat[mind.themes_stat.Count - 1] = new KeyValuePair<string, int>(mind.themes_stat[mind.themes_stat.Count - 1].Key, mind.themes_stat[mind.themes_stat.Count - 1].Value + 1);
        //}
    }
}
