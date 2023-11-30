using System.Reflection;

namespace Awesome.AI.Web.Helpers
{
    public class PathHelper
    {
        public static string Root
        {
            get
            {
                string nd = Path.DirectorySeparatorChar.ToString();
                string application = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string directory = Directory.GetParent(application).Parent.Parent.FullName;

#if DEBUG
                string rootPath = directory + nd;
#else
                string rootPath = application + nd;
#endif

                return rootPath;
            }
        }

        public static string PathError
        {
            get
            {
                string path = Root + "DataFiles\\error.xml";

                return path;
            }
        }

        public static string PathMessage
        {
            get
            {
                string path = Root + "DataFiles\\msg.xml";

                return path;
            }
        }

        public static string PathSettings
        {
            get
            {
                string path = Root + "DataFiles\\settings.xml";

                return path;
            }
        }
    }
}
