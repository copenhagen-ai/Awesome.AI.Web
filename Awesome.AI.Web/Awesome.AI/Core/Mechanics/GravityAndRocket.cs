﻿using Awesome.AI.Common;
using Awesome.AI.Interfaces;
using Awesome.AI.Variables;
using static Awesome.AI.Variables.Enums;

namespace Awesome.AI.Core.Mechanics
{
    public class GravityAndRocket : IMechanics
    {
        public double peek_momentum { get; set; }
        public double peek_norm { get; set; }
        public double p_100 { get; set; }
        public double d_100 { get; set; }
        public double p_90 { get; set; }
        public double d_90 { get; set; }
        public double p_curr { get; set; }
        public double p_prev { get; set; }
        public double d_curr { get; set; }
        public double d_prev {  get; set; }
        public double Fsta { get; set; }
        public double Fdyn { get; set; }
        public double m_out_high_c { get; set; }
        public double m_out_low_c { get; set; }
        public double d_out_high { get; set; }
        public double d_out_low { get; set; }
        public double posx_high { get; set; }
        public double posx_low { get; set; }
        
        private TheMind mind;
        private GravityAndRocket() { }
        public GravityAndRocket(TheMind mind, Params parms)
        {
            this.mind = mind;

            posxy = CONST.STARTXY;

            //m_out_high = -1.0E20d;
            //m_out_low = 1.0E20d;
            //d_out_high = -1.0E20d;
            //d_out_low = 1.0E20d;
            posx_high = -1.0E20d;
            posx_low = 1.0E20d;
        }

        public FUZZYDOWN FuzzyMom
        {
            get { return d_curr.ToFuzzy(mind); }
        }

        public HARDDOWN HardMom
        {
            //return p_curr.ToDownPrev(p_prev, mind);
            //return p_curr.ToDownZero(mind);

            //return p_delta.ToDownPrev(p_delta_prev, mind);
            get { return d_curr.ToDownZero(mind); }            
        }

        private double posxy { get; set; }
        public double POS_XY
        {
            get 
            {
                return -1d;
            }
        }

        public void Normalize()
        {
            throw new NotImplementedException();
        }

        public void Peek(UNIT curr)
        {
            throw new NotImplementedException();
        }

        public void CalcPattern1(PATTERN pattern, int cycles)
        {
            if (mind.z_current != "z_mech")
                return;

            if (pattern != PATTERN.MOODGENERAL)
                return;

            throw new NotImplementedException();
        }

        public void CalcPattern2(PATTERN pattern, int cycles)
        {
            if (mind.z_current != "z_mech")
                return;

            if (pattern != PATTERN.MOODGOOD)
                return;

            throw new NotImplementedException();
        }

        public void CalcPattern3(PATTERN pattern, int cycles)
        {
            if (mind.z_current != "z_mech")
                return;

            if (pattern != PATTERN.MOODBAD)
                return;

            throw new NotImplementedException();
        }

        //public double VariableOld(UNIT curr)
        //{
        //    /*
        //     * I guess this is a changeable function, for now it is just the one I know to calculate force
        //     * 
        //     * earth mass:               5.972×10^24 kg
        //     * sun mass:                 1.989×10^30 kg
        //     * earth gravity:                  9.807 m/s²
        //     * distance sun:             148.010.000 km
        //     * distance moon:             3.844×10^5 km 3.844e5;
        //     * distance mercury(avg):    ~58 million km (~0.39 AU)
        //     * earth radius:                   6,371 km
        //     * millinium falken:            40000.0d kg
        //     * */

        //    if (curr.IsNull())
        //        throw new Exception("Variable");

        //    if (curr.IsIDLE())
        //        throw new Exception("Variable");

        //    double dist = mind.calc.NormalizeRange(curr.LowAtZero, 0.0d, 100.0d, 0.0d, 50000000000.0d);
        //    double mass_M = 5.972E24d;
        //    double mass_m = 40000.0d;

        //    //Gravitational Constant (G)
        //    double G = Constants.GRAV_CONST;

        //    // FORMEL: ~F = (G * (m * M) / r^2) * ~r
        //    double grav = G * ((mass_m * mass_M) / (dist * dist));

        //    return grav;
        //}

        //public void CalcPatternOld(MECHVERSION version)
        //{
        //    if (version != MECHVERSION.OLD)
        //        return;
        //    /*
        //     * still experimental..
        //     * I know its not using a black hole, but it should be the same principle outside the event horizon???
        //     * */

        //    double mod = ModifierOld(mind.curr_unit.credits, mind.parms.shift);
        //    double m = 40000.0d;
        //    double dt = 100.0d;    //delta time
        //    double Fnet = mind.curr_unit.Variable * mod;

        //    //F=m*a
        //    //a=dv/dt
        //    //F=(m*dv)/dt
        //    //F*dt=m*dv
        //    //dv=(F*dt)/m

        //    double deltaVel = (Fnet * dt) / m;            //delta velocity

        //    //momentum: p = m * v
        //    p_delta_prev = p_delta;
        //    p_delta = (m) * deltaVel;
        //    p_curr += p_delta;

        //    if (p_curr <= m_out_low) m_out_low = p_curr;
        //    if (p_curr > m_out_high) m_out_high = p_curr;

        //    if (p_delta <= d_out_low) d_out_low = p_delta;
        //    if (p_delta > d_out_high) d_out_high = p_delta;

        //    if (double.IsNaN(deltaVel))
        //        throw new Exception("Calculate");
        //}

        //public double ModifierOld(double credits, double shift)
        //{
        //    Calc calc = mind.calc;

        //    double _c = 10.0d - credits;
        //    double x = 5.0d - _c + shift;
        //    double mod = calc.Logistic(x);

        //    return mod < 0.5 ? -1d : 1d;
        //}        
    }
}

