﻿using Awesome.AI.Core;
using Awesome.AI.Helpers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;
using static Awesome.AI.Helpers.Enums;
using static Google.Protobuf.WellKnownTypes.Field.Types;

namespace Awesome.AI.Common
{
    public static class Extensions
    {
        public static bool IsNull<T>(this T source)
        {
            return source == null;
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count() == 0;
        }

        public static bool IsNullOrEmpty(this string source)
        {
            return source == null || source.Count() == 0;
        }

        public static bool HasValue(this string source)
        {
            return source != null && source != "";
        }

        public static bool HasValue(this double value)
        {
            return !Double.IsNaN(value) && !Double.IsInfinity(value);
        }


        private static Random rng = new Random();
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static double Convert(this double _x, TheMind mind)
        {
            double _l = 0.0d;
            double _h = 100.0d;
            
            double res = mind.calc.NormalizeRange(_x, _l, _h, Constants.MIN, Constants.MAX);

            return res;
        }

        //public static bool TheHack(this bool _b, TheMind mind)
        //{
        //    /*
        //     * >> this is the hack/cheat <<
        //     * */
        //    bool do_hack = mind.parms.hack == HACKMODES.HACK;
        //    if (do_hack)
        //        return !_b;
        //    return _b;
        //}

        public static SOFTDOWN ToFuzzy(this double deltaMom, TheMind mind)
        {
            double high = mind.mech.d_out_high;
            double low = mind.mech.d_out_low;

            double norm = mind.calc.NormalizeRange(deltaMom, low, high, 0.0d, 100.0d);

            if (mind.parms.hack == HACKMODES.HACK)
                norm = 100.0d - norm;

            switch (norm)
            {
                case < 20.0d: return SOFTDOWN.VERYNO;
                case < 40.0d: return SOFTDOWN.NO;
                case < 60.0d: return SOFTDOWN.DUNNO;
                case < 80.0d: return SOFTDOWN.YES;
                case < 100.0d: return SOFTDOWN.VERYYES;
                default: throw new NotSupportedException("ToFuzzy");
            }
        }

        public static bool PeriodDown(this List<HARDDOWN> Ratio, TheMind mind)
        {
            /*
             * indifferent of the direction
             * */

            int count_no = Ratio.Count(x=>x == HARDDOWN.NO);
            int count_yes = Ratio.Count(x=>x == HARDDOWN.YES);

            bool res = count_no >= count_yes;

            //if (mind.parms.hack == HACKMODES.HACK)
            //    res = !res;

            return res;
        }

        //public static bool TheChoice(this TheMind mind)
        //{
        //    /*
        //     * this filter can be on or off, just have to tweek HardMom and ToDownZero/ToDownPrev
        //     * personally i think its nice to not have words like choice in the algorithm
        //     * */

        //    bool hello = mind.goodbye.IsNo();
        //    bool go_down = mind.dir.DownHard.IsYes();
        //    bool? go_up = null;

        //    double[] rand = mind.rand.MyRandomDouble(1);
        //    bool down = rand[0] < 0.5d;

        //    //introduce logic error.. or hack
        //    if (Constants.LogicError == LOGICERROR.TYPE1)
        //        return true;

        //    if (Constants.LogicError == LOGICERROR.TYPE2)
        //        go_up = hello && !go_down;

        //    //if (Constants.LogicError == LOGICERROR.TYPE3)
        //    //    go_up = hello && go_down;

        //    if (Constants.LogicError == LOGICERROR.RANDOM)
        //        go_up = !down ? hello && go_down : hello && !go_down;

        //    //not tested, very experimental
        //    if (Constants.LogicError == LOGICERROR.QUANTUM1)
        //        return true;

        //    //not tested, very experimental
        //    if (Constants.LogicError == LOGICERROR.QUANTUM2)
        //        go_up = hello && mind.quantum.MyXOR(go_down, go_down);

        //    if (go_up == null)
        //        throw new Exception("TheChoice");

        //    return (bool)go_up;
        //}

        public static bool Direction(this TheMind mind)
        {
            /*
             * this filter can be on or off, just have to tweek HardMom and ToDownZero/ToDownPrev
             * */

            bool hello = mind.goodbye.IsNo();
            bool go_up = hello && mind.dir.DownHard.IsNo();

            return go_up;
        }

        public static HARDDOWN ToDownZero(this double deltaMom, TheMind mind)
        {
            /*
             * "NO", is to say no to going downwards
             * */

            bool res = deltaMom <= 0.0d;

            double[] rand = mind.rand.MyRandomDouble(1);
            bool down = rand[0] < 0.5d;

            return Logic(res, down, mind);
        }

        public static HARDDOWN ToDownPrev(this double deltaMom, double prev, TheMind mind)
        {
            /*
             * "NO", is to say no to going downwards
             * */

            bool res = deltaMom <= prev;

            double[] rand = mind.rand.MyRandomDouble(1);
            bool down = rand[0] < 0.5d;

            return Logic(res, down, mind);
        }

        public static HARDDOWN Logic(bool res, bool down, TheMind mind)
        {
            //works
            if (Constants.Logic == LOGICTYPE.BOOLEAN)
                return res ? HARDDOWN.NO : HARDDOWN.YES;

            //wrong logic, experimental
            if (Constants.Logic == LOGICTYPE.RANDOM)
                return res ? down.ToDirection() : HARDDOWN.NO;

            //wrong logic, experimental
            if (Constants.Logic == LOGICTYPE.QUANTUM1)
                return res ? mind.quantum.MySuperposition().ToDirection() : HARDDOWN.NO;

            //wrong logic, experimental
            if (Constants.Logic == LOGICTYPE.QUANTUM2)
                return res ? mind.quantum.MyXOR(res, res).ToDirection() : HARDDOWN.NO;

            throw new Exception("ToDownPrev");
        }

        public static HARDDOWN ToDirection(this bool _q)
        {
            return _q ? HARDDOWN.YES : HARDDOWN.NO;
        }

        public static bool IsYes(this HARDDOWN _q)
        {
            return _q == HARDDOWN.YES;
        }

        public static bool IsNo(this HARDDOWN _q)
        {
            return _q == HARDDOWN.NO;
        }
    }
}
