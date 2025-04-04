﻿using Awesome.AI.Core;

namespace Awesome.AI.Common
{
    public class Calc
    {
        private TheMind mind;
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
            double normalized = (((val - valmin) / (valmax - valmin)) * (ranmax - ranmin)) + ranmin;
            return normalized;
        }

        public double Denormalize(double normalized, double min, double max)
        {
            double denormalized = (normalized * (max - min) + min);
            return denormalized;
        }

        public bool MyChanceTo100(double val, bool gt = true)
        {
            int[] num = mind.rand.MyRandomInt(1, 100);
            return gt ? val > num[0] : val < num[0];
        }

        public bool MyChanceTo1(double val, bool gt = true)
        {
            int[] num = mind.rand.MyRandomInt(1, 1);
            return gt ? val > num[0] : val < num[0];
        }

        public bool ChanceTo100(double val, bool gt = true)
        {
            int num = mind.rand.RandomInt(100);
            return gt ? val > num : val < num;
        }

        public bool ChanceTo1(double val, bool gt = true)
        {
            double num = mind.rand.RandomDouble(0.0d, 1.0d);
            return gt ? val > num : val < num;
        }

        public double ToPercent(double num, double high)
        {
            double res = 0.0d;
            res = mind.calc.Normalize(num, 0.0d, high) * 100.0d;

            return res;
        }

        public double Percentage(double num, double high)
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

        public int HighestIndex(double[] arr)
        {
            double max = arr.Max();
            int index = arr.ToList().IndexOf(max);
            return index;
        }

        public double Reciprocal(double x)
        {
            if (x < 0.0d)
                throw new Exception("Reciprocal");
            
            if (x == 0.0d)
                throw new Exception("Reciprocal");

            return 1 / x;
        }        

        public double EventHorizon(double r)
        {
            /*
             * Gravitational Time Dilation
             * */

            if (r < 0.0d)
                throw new Exception("EventHorizon");

            if (r == 0.0d)
                throw new Exception("EventHorizon");

            double t_observer = 0.0d;
            double t_distant = 1.0d;
            double Rs = 2.0d;

            if (r <= Rs)
                r = Rs;

            double part = 1.0d - Rs / r;
            t_observer = t_distant * Math.Sqrt(part);
            
            return t_observer;
        }

        public double Linear(double x, double a, double b)
        {
            if (x < 0.0d)
                throw new Exception("Linear");

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
                throw new Exception("Pyth");
            if (_b <= 0.0d)
                throw new Exception("Pyth");

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
                throw new Exception("PythNear");
            if (theta < -180.0d)
                throw new Exception("PythNear");
            if (hypotenuse <= 0.0d)
                throw new Exception("PythNear");

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
                throw new Exception("PythFar");
            if (theta <= 0.0d)
                throw new Exception("PythFar");
            if (hypotenuse <= 0.0d)
                throw new Exception("PythFar");

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
