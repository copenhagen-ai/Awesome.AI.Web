﻿using Awesome.AI.Common;
using Awesome.AI.Interfaces;
using Awesome.AI.Variables;
using static Awesome.AI.Variables.Enums;

namespace Awesome.AI.Core.Mechanics
{
    public class _TheHill : IMechanics
    {
        public double momentum { get; set; }
        public double deltaMom { get; set; }
        private double deltaMomPrev {  get; set; }

        public double Fsta { get; set; }
        public double Fdyn { get; set; }
        public double m_out_high { get; set; }
        public double m_out_low { get; set; }
        public double d_out_high { get; set; }
        public double d_out_low { get; set; }
        public double posx_high { get; set; }
        public double posx_low { get; set; }
        
        public double res_x { get; set; } = 5.0d;
                
        private TheMind mind;
        private _TheHill() { }
        public _TheHill(TheMind mind, Params parms)
        {
            this.mind = mind;
            
            posxy = Constants.STARTXY;//10;

            m_out_high = -1000.0d;
            m_out_low = 1000.0d;
            d_out_high = -1000.0d;
            d_out_low = 1000.0d;
            posx_high = -1000.0d;
            posx_low = 1000.0d;
        }

        public FUZZYDOWN FuzzyMom
        {
            get { return deltaMom.ToFuzzy(mind); }
        }

        public HARDDOWN HardMom
        {
            
            get 
            {
                if (Constants.Logic == LOGICTYPE.BOOLEAN)
                    //return deltaMom.ToDownPrev(deltaMomPrev, mind);
                    return deltaMom.ToDownZero(mind);

                if (Constants.Logic == LOGICTYPE.QUBIT)
                    //return deltaMom.ToDownPrev(deltaMomPrev, mind);
                    return deltaMom.ToDownZero(mind);

                throw new Exception("HardMom");
            }
        }

        public double HighestVar
        {
            get { return Variable(UNIT.GetLow); }
        }

        public double LowestVar
        {
            get { return Variable(UNIT.GetHigh); }
        }

        private double posxy { get; set; }
        public double POS_XY
        {
            get
            {
                //its a hack, yes its cheating..
                double boost = mind.parms.boost;

                //if (mind.goodbye.IsNo())
                //    posxy = Constants.STARTXY + (boost * momentum);
                //else
                //    posxy += (boost * momentum);

                if (mind.goodbye.IsNo())
                    posxy = Constants.STARTXY + (boost * deltaMom);
                else
                    posxy += (boost * deltaMom);

                //POS_X = 10.0d + (boost * momentum);

                if (posxy < Constants.LOWXY)
                    posxy = Constants.LOWXY;
                if (posxy > Constants.HIGHXY)
                    posxy = Constants.HIGHXY;

                if (posxy <= posx_low) posx_low = posxy;
                if (posxy > posx_high) posx_high = posxy;

                return 10.0d - posxy;
            }
        }

        //Weight
        public double Variable(UNIT _c)
        {
            /*
             * This is a changeable function.
             * 
             * Weight
             * W = m * g
             * */
            if (_c.IsNull())
                throw new Exception("Variable");

            if (_c.IsIDLE())
                throw new Exception("Variable");

            double earth_gravity = Constants.GRAVITY;
            double mass = mind.parms.mass;
            
            double res = (mass * earth_gravity) * (_c.HighAtZero / 100.0d);

            return res;
        }

        private double SlopeInDegrees(double x)
        {
            double acc_slope, _x, _y;

            MyVector2D calc = new MyVector2D();
            MyVector2D _slope;
            
            acc_slope = mind.calc.SlopeCoefficient(x, Vars.var_a, Vars.var_b);
            _x = 1.0d;
            _y = acc_slope;
            _slope = calc.ToPolar(new MyVector2D(_x, _y, null, null));
            double acc_degree = _slope.theta_in_degrees;
            
            return acc_degree;
        }
        
        public void Calculate()
        {
            Check(Vars.var_a, Vars.var_b, Vars.var_c);

            MyVector2D calc = new MyVector2D();
            MyVector2D res, sta = new MyVector2D(), dyn = new MyVector2D();

            //double res_x_save = Constants.STARTXY + res_x_prev;
            double acc_degree = SlopeInDegrees(res_x);

            sta = ApplyStatic(acc_degree);
            
            if (mind.goodbye.IsNo())
                dyn = ApplyDynamic(acc_degree);

            res = calc.Add(sta, dyn);
            res_x = res.xx;
            res = calc.ToPolar(res);

            if (res_x < 0.0d) res_x = 0.0d;
            if (res_x > 10.0d) res_x = 10.0d;

            double acc = res.yy < 0.0d ? res.magnitude : -res.magnitude;

            double m = mind.parms.mass;
            //double dt = mind.parms.delta_time;
            double deltaT = mind.parms.delta_time;

            //F=m*a
            //a=dv/dt
            //dv=a*dt
            //double dv = acc * dt;
            double deltaVel = acc * deltaT;

            deltaMomPrev = deltaMom;
            deltaMom = m * deltaVel;
            momentum += deltaMom;

            if (momentum <= m_out_low) m_out_low = momentum;
            if (momentum > m_out_high) m_out_high = momentum;

            if (deltaMom <= d_out_low) d_out_low = deltaMom;
            if (deltaMom > d_out_high) d_out_high = deltaMom;

            //if (momentum <= 0.0d)
            //    throw new Exception("momentum less than 0.0");

            //double acc = res.theta_in_degrees < 0.0d ? res.magnitude : -res.magnitude;
            //double acc = dyn.magnitude > sta.magnitude ? -res.magnitude : res.magnitude;
            //double acc = sta.magnitude - dyn.magnitude;
            //double acc = res.magnitude;

            //velocity = dv;
            //momentum: p = m * v
            //momentum += m * velocity;
            //momentum += m * dv;
            //velocity += dv;
        }

        public MyVector2D ApplyStatic(double acc_degree)
        {
            double acc_degree_positive = acc_degree < 0.0d ? -acc_degree : acc_degree;
            double angle_sta = -90.0d;
            double angle_com_y_vec = -90.0d - acc_degree_positive;//-135
            double angle_com_y_pyth = 90.0d - acc_degree_positive;//-135

            double force_sta = HighestVar;
            double force_com_y = mind.calc.PythNear(angle_com_y_pyth, force_sta);

            MyVector2D calc = new MyVector2D();
            MyVector2D _static = calc.ToCart(new MyVector2D(null, null, force_sta, calc.ToRadians(angle_sta)));
            MyVector2D _N = calc.ToCart(new MyVector2D(null, null, force_com_y, calc.ToRadians(angle_com_y_vec + 180.0d)));
            MyVector2D _fN = calc.ToPolar((calc.Add(_static, _N)));

            double m = mind.parms.mass;
            double u = Friction(true, 0.0d, mind.parms.shift);
            double N = m * Constants.GRAVITY;

            double Ffriction = u * N;
            double Fapplied = _fN.magnitude;
            double Fnet = Fapplied - Ffriction;

            if (Fnet <= Constants.VERY_LOW)
                Fnet = Constants.VERY_LOW;

            MyVector2D _res = calc.ToCart(new MyVector2D(null, null, Fnet, _fN.theta_in_radians));

            return _res;
        }

        public MyVector2D ApplyDynamic(double acc_degree)
        {
            UNIT curr_unit = mind.curr_unit;
            
            if (curr_unit.IsNull())
                throw new Exception("ApplyDynamic");

            double acc_degree_positive = acc_degree < 0.0d ? -acc_degree : acc_degree;
            double angle_dyn = 90.0d + acc_degree_positive;

            double max = HighestVar;
            double force_dyn = max - curr_unit.Variable;

            MyVector2D calc = new MyVector2D();
            MyVector2D dynamic = new MyVector2D(null, null, force_dyn, calc.ToRadians(angle_dyn));

            double m = mind.parms.mass;
            double u = Friction(false, curr_unit.credits, mind.parms.shift);
            double N = m * Constants.GRAVITY;

            double Ffriction = u * N;
            double Fapplied = dynamic.magnitude;
            double Fnet = Fapplied - Ffriction;

            if (Fnet <= Constants.VERY_LOW)
                Fnet = Constants.VERY_LOW;
            
            MyVector2D _res = calc.ToCart(new MyVector2D(null, null, Fnet, calc.ToRadians(angle_dyn)));

            return _res;
        }

        public double Friction(bool is_static, double credits, double shift)
        {
            /*
             * friction coeficient
             * should friction be calculated from position???
             * */

            if (is_static)
                return Constants.BASE_FRICTION;

            Calc calc = mind.calc;

            double _c = 10.0d - credits;
            double x = 5.0d - _c + shift;
            double friction = calc.Logistic(x);

            return friction / 2.0d;
        }

        //private double DeltaV(double a, double dt)
        //{
        //    //F=m*a
        //    //a=dv/dt
        //    //dv=a*dt
        //    double dv = a * dt;
        //    return dv;
        //}

        //private double DeltaT()
        //{
        //    //most of the time this is true

        //    return 0.002d;
        //}

        private bool Check(double _a, double _b, double _c)
        {
            double _x1, _x2;
            mind.calc.Roots(null, _a, _b, _c, out _x1, out _x2);

            bool ok = _x1 == -10.0d & _x2 == 10.0d;

            if (!ok)
                throw new Exception("Check");

            return ok;
        }
    }
}

