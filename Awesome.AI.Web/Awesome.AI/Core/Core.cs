using Awesome.AI.Common;
using static Awesome.AI.Helpers.Enums;

namespace Awesome.AI.Core
{
    public class TheCurve
    {
        /*
         * this is the Go/NoGo class
         * actually not part of the algorithm
         * */
        TheMind mind;
        private TheCurve() { }
        public TheCurve(TheMind mind)
        {
            this.mind = mind;
        }

        public bool OK(out double pain)
        {
            try
            {
                double _e = mind.parms._mech.EXIT();
                pain = mind.calc.Reciprocal(_e);
                    
                return pain < mind.parms.max_pain;
            }
            catch (Exception e)//thats it
            {
                pain = mind.parms.max_pain;
                return false;
            }
        }
    }

    public class Core
    {
        TheMind mind;
        private Core() { }
        public Core(TheMind mind)
        {
            this.mind = mind;
        }

        public void UpdateCredit()
        {
            List<UNIT> list = mind.mem.UNITS_VAL();
            foreach (UNIT _u in list)//this could be a problem with many hubs
            {
                if (_u.root == mind.curr_unit.root)
                    continue;

                double nrg = mind.parms.update_nrg;
                _u.credits += nrg;

                if (_u.credits > _u.max_nrg)
                    _u.credits = _u.max_nrg;
            }

            mind.curr_unit.credits -= 1.0d;
            if (mind.curr_unit.credits < 0.0d)
                mind.curr_unit.credits = 0.0d;
        }
        
        public double FrictionCoefficient(bool is_static, double credits)
        {
            //should friction be calculated from position???

            if (is_static)
                return mind.parms.base_friction;
            
            //friction = 
            //    credits > 8.0d ? 0.33d :
            //    credits > 5.0d ? mind.parms.base_friction :
            //                     1.0d;

            Calc calc = new Calc(mind);

            double friction = calc.Linear(credits, -1.0d, 10d) / 10;

            return friction;
        }

        public void AnswerQuestion()
        {
            /*
             * ..or if all goals are fulfilled?
             * ..or if can make consious choise
             * should there be some procedure for this(unlocking)?
             * */

            if ((mind.epochs + 0) % (60 * mind.parms.runtime) == 0)
                mind.theanswer.root = "It does not";

            if ((mind.epochs + 1) % (60 * mind.parms.runtime) == 0)
                mind.theanswer.root = "It does not";

            if ((mind.epochs + 2) % (60 * mind.parms.runtime) == 0)
                mind.theanswer.root = "It does not";

            if ((mind.epochs + 3) % (60 * mind.parms.runtime) == 0)
                mind.theanswer.root = "It does not";

            string answer = mind.theanswer.root;
            if (answer == null)
                throw new ArgumentNullException();

            mind.goodbye = answer == "It does not" ? THECHOISE.YES : THECHOISE.NO;
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
