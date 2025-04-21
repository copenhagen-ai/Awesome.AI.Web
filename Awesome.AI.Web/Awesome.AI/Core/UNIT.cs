﻿using Awesome.AI.CoreInternals;
using Awesome.AI.Variables;
using static Awesome.AI.Variables.Enums;

namespace Awesome.AI.Core
{
    public class UNIT
    {
        /*
         * maybe later there will be other types, like: SOUND, IMAGE, VIDEO
         * */

        public Ticket ticket = new Ticket("NOTICKET");
        private UNITTYPE unit_type { get; set; }
        public LONGTYPE long_deci_type { get; set; }
        public string hub_guid { get; set; }//name
        public string root { get; set; }//name
        public string data { get; set; }//data
        public double credits { get; set; }

        private TheMind mind;
        private UNIT() { }
        public UNIT(TheMind mind)
        {
            this.mind = mind;
        }

        private double dex = -1.0d;
        public double Index
        {
            get { return dex; }
            set { dex = value; }
        }

        public double Variable
        {
            get
            {
                bool high_at_zero = mind.parms[mind.current].high_at_zero;

                return high_at_zero ? HighAtZero : LowAtZero;
            }
        }

        public double LowAtZero
        {
            get { return Index; }
        }

        public double HighAtZero
        {
            get { return 100.0d - Index; }
        }

        public static UNIT GetHigh
        {
            get { return Create(null, "GUID", Constants.MAX, "MAX", "DATA", "NONE", UNITTYPE.MAX, LONGTYPE.NONE); }
        }

        public static UNIT GetLow
        {
            get { return Create(null, "GUID", Constants.MIN, "MIN", "DATA", "NONE", UNITTYPE.MIN, LONGTYPE.NONE); }
        }

        public bool IsLowCut
        {
            get { return mind.filters.LowCut(this); }
        }

        public bool CreditOK
        {
            get { return mind.filters.Credits(this); }
        }

        public bool IsValid
        {
            get
            {
                switch (mind.parms[mind.current].validation)
                {
                    case VALIDATION.BOTH:
                        return mind._internal.Valid(this) && mind._external.Valid(this);
                    case VALIDATION.EXTERNAL:
                        return mind._external.Valid(this);
                    case VALIDATION.INTERNAL:
                        return mind._internal.Valid(this);
                    default:
                        throw new Exception("IsValid");
                }
            }
        }

        public HUB HUB
        {
            get
            {
                if (IsIDLE())
                    return HUB.Create("GUID", "IDLE", new List<UNIT>(), TONE.RANDOM);

                STATE state = mind.parms[mind.current].state;

                try {
                    return mind.mem.HUBS_ALL(state).Where(x => x.hub_guid == this.hub_guid).First();
                }
                catch {
                    return HUB.Create("GUID", "IDLE", new List<UNIT>(), TONE.RANDOM);
                }
            }
        }

        public List<UNIT> REL
        {
            get
            {
                List<UNIT> rel = HUB.units;

                return rel;
            }
        }

        public static UNIT Create(TheMind mind, string h_guid, double index, string root, string data, string ticket, UNITTYPE ut, LONGTYPE lt)
        {
            UNIT _w = new UNIT() { mind = mind, hub_guid = h_guid, Index = index, root = root, data = data, unit_type = ut, long_deci_type = lt };

            if (ticket != "")
                _w.ticket = new Ticket(ticket);

            _w.credits = Constants.MAX_CREDIT;

            return _w;
        }

        public void Adjust(double sign, double dist)
        {
            if (dist < Constants.ALPHA)
                return;

            double rand = mind.rand.MyRandomDouble(10)[5];

            Index += rand * Constants.ETA * sign;
        }


        public static UNIT IDLE_UNIT(TheMind mind)
        {
            return Create(mind, "GUID", -1d, "ROOT", "DATA", "NONE", UNITTYPE.IDLE, LONGTYPE.NONE);
        }

        public bool IsUNIT() => unit_type == UNITTYPE.JUSTAUNIT;

        public bool IsIDLE() => unit_type == UNITTYPE.IDLE;

        public bool IsDECISION() => unit_type == UNITTYPE.DECISION;

        public bool IsQUICKDECISION() => unit_type == UNITTYPE.QDECISION;

        

        //UNIT next = null;
        //public UNIT Next
        //{
        //    get
        //    {
        //        if (!next.IsNull())
        //            return next;

        //        List<UNIT> units = mind.mem.UNITS_VAL();
        //        units = units.OrderByDescending(x => x.Variable).ToList();
        //        units = units.Where(x => !x.IsSPECIAL()).ToList();
        //        units = units.Where(x => !x.IsLowCut).ToList();
        //        units = units.Where(x => x.Variable < this.Variable).ToList();

        //        next = units.FirstOrDefault();
        //        if (next.IsNull())
        //            next = new UNIT() { type = TYPE.NOTNEXTPREV };

        //        return next;
        //    }
        //}

        //UNIT prev = null;
        //public UNIT Prev
        //{
        //    get
        //    {
        //        if (!prev.IsNull())
        //            return prev;

        //        List<UNIT> units = mind.mem.UNITS_VAL();
        //        units = units.OrderByDescending(x => x.Variable).ToList();
        //        units = units.Where(x => !x.IsSPECIAL()).ToList();
        //        units = units.Where(x => !x.IsLowCut).ToList();
        //        units = units.Where(x => x.Variable > this.Variable).ToList();

        //        prev = units.LastOrDefault();
        //        if (prev.IsNull())
        //            prev = new UNIT() { type = TYPE.NOTNEXTPREV };

        //        return prev;
        //    }
        //}

        //public bool IsLEARNING()
        //{
        //    return
        //        type == TYPE.LEARNING;
        //}

        //public bool IsPERSUE()
        //{
        //    return
        //        type == TYPE.PERSUE;
        //}

        //public bool IsNEXTPREV()
        //{
        //    return
        //        type != TYPE.NOTNEXTPREV;
        //}

        //public bool IsLEARNINGorPERSUE()
        //{
        //    return
        //        type == TYPE.LEARNING ||
        //        type == TYPE.PERSUE;
        //}

        //public bool IsSPECIAL()
        //{
        //    return
        //        type == TYPE.IDLE ||
        //        type == TYPE.LEARNING ||
        //        type == TYPE.PERSUE;
        //}

    }
}
