using Awesome.AI.Core;
using Awesome.AI.CoreHelpers;
using System.Linq.Expressions;
using System.Reflection;
using static Awesome.AI.Helpers.Enums;

namespace Awesome.AI.Common
{
    public class Common
    {
        TheMind mind;
        private Common() { }
        public Common(TheMind mind)
        {
            this.mind = mind;
        }

        public UNIT highest_filter = null;
        public UNIT HighestForceHighPass()
        {
            if (highest_filter != null)
                return highest_filter;

            UNIT unit = mind.mem.UNITS_VAL().Where(x => mind.filters.HighPass(x)).OrderByDescending(x=>x.Variable).FirstOrDefault();
            
            highest_filter = unit;
            return highest_filter;
        }

        public UNIT highest_force = null;
        public UNIT HighestForce()
        {
            if (highest_force != null)
                return highest_force;

            UNIT unit = mind.mem.UNITS_VAL().OrderByDescending(x => x.Variable).FirstOrDefault();
            
            highest_force = unit;
            return highest_force;
        }

        public UNIT lowest_force = null;
        public UNIT LowestForce()
        {
            if (lowest_force != null)
                return lowest_force;

            UNIT unit = mind.mem.UNITS_VAL().OrderBy(x => x.Variable).FirstOrDefault();
            
            lowest_force = unit;
            return lowest_force;
        }

        public void Reset() 
        {
            highest_filter = null;        
            highest_force = null;        
            lowest_force = null;
        }

        public string GetMemberName<T>(Expression<Func<T>> memberExpression)
        {
            MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
            return expressionBody.Member.Name;
        }

        public Type GetType(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            return t;
        }

        public object GetInstance(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            if (t == null)
                return null;
            return Activator.CreateInstance(t);
        }

        public string Root
        {
            get
            {
                string nd = Path.DirectorySeparatorChar.ToString();
                string applicationDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string rootPath = "";

                if(mind.parms.debug)
                    rootPath = Directory.GetParent(applicationDirectory).Parent.Parent.FullName + nd;
                else
                    rootPath = applicationDirectory + nd;

                return rootPath;//.Replace("Skrivebord", "Desktop");
            }
        }

        public string PathSetup
        {
            get
            {
                string path = Root + "Awesome.AI\\Data\\setup_1.xml";
                if(mind.parms.matrix_type == MATRIX.GPT)
                    path = Root + "Awesome.AI\\Data\\setup_2.xml";

                return path;
                /*
                string path;

                switch (Params.t_select)
                {
                    case "1": path = Statics.Root + "Data\\setup_1.xml"; break;
                    case "12": path = Statics.Root + "Data\\setup_12.xml"; break;
                    case "123": path = Statics.Root + "Data\\setup_123.xml"; break;
                    case "1234": path = Statics.Root + "Data\\setup_1234.xml"; break;
                    default:
                        throw new Exception();
                }

                return path;*/
            }
        }
    }
}
