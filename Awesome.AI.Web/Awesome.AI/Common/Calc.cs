﻿using Awesome.AI.Core;
using Awesome.AI.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace Awesome.AI.Common
{
    public class Calc
    {
        TheMind mind;
        private Calc() { }
        public Calc(TheMind mind) 
        {
            this.mind = mind;
        }

        public double Normalize(double value, double valmin, double valmax)
        {
            double normalized = (value - valmin) / (valmax - valmin);
            return normalized;
        }

        public double NormalizeRange(double val, double valmin, double valmax, double ranmin, double ranmax)
        {
            return (((val - valmin) / (valmax - valmin)) * (ranmax - ranmin)) + ranmin;
        }

        public double Denormalize(double normalized, double min, double max)
        {
            double denormalized = (normalized * (max - min) + min);
            return denormalized;
        }

        public bool MyChance0to100(double val, bool gt)
        {
            int num = mind.calc.MyRandom(100);
            return gt ? num > val : num < val;
        }

        public bool Chance0to100(double val, bool gt)
        {
            int num = mind.calc.RandomInt(100);
            return gt ? num > val : num < val;
        }

        public bool Chance0to1(double val, bool gt)
        {
            double num = mind.calc.RandomDouble(0.0d, 1.0d);
            return gt ? num > val : num < val;
        }

        public string FormatDouble(double d, int decimals)
        {
            string s = "" + d;
            string res = "";
            if (s.Contains(','))
            {
                res = s.Split(',')[0];
                char[] arr = s.Split(',')[1].ToCharArray();
                if (decimals > 0)
                    res += ",";
                for (int i = 0; i < decimals; i++)
                    res += i < arr.Length ? "" + arr[i] : "0";
            }
            else
            {
                if (decimals > 0)
                    s += ",";
                for (int i = 0; i < decimals; i++)
                    s += "0";
                res = s;
            }

            return res;
        }

        public double ToPercent(double num, double high)
        {
            double res = 0.0d;
            res = mind.calc.Normalize(num, 0.0f, high) * 100.0d;

            return res;
        }

        public double GetPercentage(double num, double high)
        {
            /*
             * How many percent is 50 from 500? 	50 / 500 * 100% = 10%
             * */

            double res = num / high * 100.0d;

            return res;
        }

        public int RoundOff(int i)
        {
            return ((int)Math.Round(i / 10.0)) * 10;
        }

        public double Round(double num, int dec, bool up)
        {
            int pow = (int)Math.Pow((double)10, (double)dec);
            double res;
            res = up ? Math.Ceiling(num * pow) / (double)pow : Math.Floor(num * pow) / (double)pow;

            return res;
        }

        public double RoundDouble(double num, int dec)
        {
            double res;
            res = Math.Round(num, dec);

            return res;
        }

        public int RoundInt(double num)
        {
            //round to nearest, up or down
            int res;
            res = System.Convert.ToInt32(num);

            return res;
        }

        public int RoundUp(double num)
        {
            int res = (int)Math.Ceiling((double)num);

            return res;
        }

        public int RoundDown(double num)
        {
            int res = (int)Math.Floor((double)num);

            return res;
        }

        //public int MyRandom(int i_max)
        //{
        //    /*
        //     * max 999
        //     */

        //    if (i_max > 999)
        //        throw new Exception();

        //    double promil = GetPromil();
        //    int res = mind.calc.RoundInt((double)i_max * promil);

        //    return res;
        //}

        //private double GetPromil()
        //{
        //    double _out = -1.0d;
        //    string rand = "";
        //    try
        //    {
        //        IMechanics mech = mind.parms._mech;

        //        if (double.IsNaN(mech.dir.d_momentum))
        //            throw new Exception();

        //        if (double.IsInfinity(mech.dir.d_momentum))
        //            throw new Exception();

        //        if (mind.cycles_all < mind.parms.first_run)//make shure the system is running before proceeding
        //            return 0.5d;

        //        _out = mech.dir.d_momentum;
        //        int i_ct = 15;
        //        string s_promil = "error";
        //        double d_promil;

        //        rand = ("" + _out).ToUpper().Replace("E", "").Replace("-", "").Replace(",", "").Replace(".", "");//this is the random part
        //        rand = rand.Substring(0, rand.Length - 2);//remove exponent

        //        do
        //        {
        //            if (rand.Count() < i_ct + 3)
        //            { i_ct--; continue; }

        //            if (i_ct < 0)
        //                return 0.5d;

        //            string _a = "" + rand.ElementAt(i_ct);
        //            string _b = "" + rand.ElementAt(i_ct + 1);//+ or -
        //            string _c = "" + rand.ElementAt(i_ct + 2);//+ or -

        //            bool ok = "0123456789".Contains(_a);
        //            ok = ok && "0123456789".Contains(_b);
        //            ok = ok && "0123456789".Contains(_c);

        //            s_promil = ok ? $"{_a}{_b}{_c}" : "error";
        //            i_ct--;
        //        } while (s_promil == "error");

        //        d_promil = double.Parse(s_promil) / 1000;

        //        return d_promil;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("rand: " + _out);
        //        Console.WriteLine("rand: " + rand);
        //        Console.WriteLine(e.Message);

        //        return -9999.9999d;
        //    }
        //}
        
        public int MyRandom(int i_max)
        {
            /*
             * max 999
             * 0 <= res <= i_max
             */

            if (i_max > 999)
                throw new Exception();

            string rand = Rand();
            
            double dec = double.Parse($"{rand[0]}{rand[1]}{rand[2]}") / 1000;
            int res = mind.calc.RoundInt((double)i_max * dec);

            return res;
        }

        private string Rand()
        {
            IMechanics mech = mind.parms._mech;

            if (double.IsNaN(mech.dir.d_momentum))
                throw new Exception();

            if (double.IsInfinity(mech.dir.d_momentum))
                throw new Exception();

            //make sure the system is running before proceeding
            if (mind.cycles_all < mind.parms.first_run)
                return "500";

            //get momentum
            string rand = "" + mech.dir.d_momentum;
            
            //remove exponent
            if (rand.ToUpper().Contains("E"))
                rand = rand.Substring(0, rand.Length - 3);
            
            //reverse, this is the random part
            string res = "";
            for (int i = rand.Length; i > 0; i--)
                res += char.IsDigit(rand[i - 1]) ? rand[i - 1] : "";
            
            return res;
        }

        private Random r1 = new Random();
        public int RandomInt(int max)
        {
            int rand = r1.Next(0, max);
            return rand;
        }

        public int RandomInt(int low, int max)
        {
            int rand = r1.Next(low, max);
            return rand;
        }

        public double RandomDouble(double min, double max)
        {
            double rand = r1.NextDouble() * (max - min) + min;
            return rand;
        }

        public int HighestIndex(double[] arr)
        {
            double max = arr.Max();
            int index = arr.ToList().IndexOf(max);
            return index;
        }

        public double Reciprocal(double x)
        {
            if (x < 0.0d)
                throw new Exception();
            
            if (x == 0.0d)
                throw new Exception();

            return 1 / x;
        }

        public double Linear(double x, double a, double b)
        {
            if (x < 0.0d)
                throw new Exception();

            double y = a * x + b;                       //y er mass

            //if (y > 2.0)                                //er egentlig ikke nødvendig
            //    throw new Exception();
            //if (y < 0.0)
            //    throw new Exception();

            return y;
        }

        public double Quadratic(double x, double a, double b, double c)
        {
            //if (x < 0.0d)
            //    throw new Exception();

            double y = a * (x * x) + (b * x) + c;         //y er mass

            //if (y >= 10.0)                              //10 er top grænsen, 10 * 10
            //    y = 10.0;
            //if (y < 0.1)                                //er egentlig ikke nødvendig
            //    throw new Exception();

            return y;
        }

        public double Logistic(double x)
        {
            /*
             * logistic function
             * f(x) = L / (1 + e^-k(x - x0))
             * 
             * standard logistic function
             * f(x) = 1/(1 + e^-x)
             * */
            double y = 1d / (1d + Math.Exp(-x));

            return y;
        }        

        public double Pyth(double _a, double _b)
        {
            if (_a <= 0.0d)
                throw new Exception();
            if (_b <= 0.0d)
                throw new Exception();

            double _dist = Math.Sqrt(_a * _a + _b * _b);

            return _dist;
        }

        public double PythNear(double theta, double hypotenuse)//hosliggende
        {
            /*
             * cos(v) = hosliggende katete / hypotenusen
             * sin(v) = modstående katete / hypotenusen
             * tan(v) = modstående katete / hosliggende katete
             * */
            if (theta > 180.0d)
                throw new Exception();
            if (theta < -180.0d)
                throw new Exception();
            if (hypotenuse <= 0.0d)
                throw new Exception();

            double _rad = ToRadiansFromDegrees(theta);
            double _cos = Math.Cos(_rad);
            double res = _cos * hypotenuse;

            return res;
        }

        public double PythFar(double theta, double hypotenuse)//modstående
        {
            /*
             * cos(v) = hosliggende katete / hypotenusen
             * sin(v) = modstående katete / hypotenusen
             * tan(v) = modstående katete / hosliggende katete
             * */
            if (theta >= 90.0d)
                throw new Exception();
            if (theta <= 0.0d)
                throw new Exception();
            if (hypotenuse <= 0.0d)
                throw new Exception();

            double _rad = ToRadiansFromDegrees(theta);
            double _sin = Math.Sin(_rad);
            double res = _sin * hypotenuse;

            return res;
        }

        public double ToRadiansFromDegrees(double angle)
        {
            double res = angle * (Math.PI / 180.0d);

            return res;
        }

        public double ToDegreesFromRadians(double radians)
        {
            double res = radians * (180.0d / Math.PI);

            return res;
        }

        public double ToDegreesFromSlope(double slope)
        {
            double res = Math.Atan(slope) * (180.0d / Math.PI);

            return res;
        }

        public double SlopeCoefficient(double x, double a, double b)
        {
            /*
             * derivative of:    f(x) = ax^2 + bx + c
             *                   f'(x)= 2ax + b
             * */

            double res = 2 * a * x + b;

            return res;
        }

        public double Integral(double x, double a, double b, double c, double k)
        {
            /*
             * derivative of:    f(x) = ax^2 + bx + c
             *                   F(x)= (1/3)ax^3 + (1/2)bx^2 + cx + k
             * */

            double res = (1/3) * a * (x*x*x) + (1/2) * b * (x*x) + c * x + k;

            return res;
        }

        public double Roots(double? _x, double _a, double _b, double _c, out double _x1, out double _x2)
        {
            _x1 = (-_b + Math.Sqrt(_b * _b - 4 * _a * _c)) / (2 * _a);
            _x2 = (-_b - Math.Sqrt(_b * _b - 4 * _a * _c)) / (2 * _a);

            if (_x.IsNull())
                return -1d;

            double res = _x >= 0.0d ? _x2 : _x1;

            return res;
        }
    }
}
