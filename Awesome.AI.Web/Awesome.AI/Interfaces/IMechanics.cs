using Awesome.AI.CoreHelpers;

namespace Awesome.AI.Interfaces
{
    public interface IMechanics
    {
        double Fsta { get; set; }
        double Fdyn { get; set; }

        double out_high { get; set; }
        double out_low { get; set; }
        double posx_high { get; set; }
        double posx_low { get; set; }
        //double res_x { get; set; }
        double POS_X { get; set; }
        double momentum { get; set; }

        Direction dir { get; set; }
        
        void CALC(/*bool process*/);
        void XPOS(/*bool process*/);
        double EXIT();
    }
}


















/*using Awesome.AI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Awesome.AI.Helpers.Enums;

namespace Awesome.AI.Interfaces
{
    public abstract class Mechanics<T> where T : new()
    {
        public static double momentum = -1000.0d;
        private static T inst = default(T);
        public abstract double ApplyForceStatic();
        public abstract double ApplyForceDynamic(UNIT curr_unit_th, THECHOISE goodbye);
        public T Inst()
        {
            if (inst.IsNull())
                inst = new T();
            return inst;
        }
    }
}/**/