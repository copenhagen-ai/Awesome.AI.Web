using System.Xml.Linq;

namespace Awesome.AI.Web.Helpers
{
    public class SettingsHelper
    {
        public static string SECRET
        {
            get
            {
                var xdoc = XElement.Load(PathHelper.PathSettings);
                var group = xdoc.Elements("security");

                foreach (XElement elem in group.Descendants())
                {
                    if (elem.Name == "setting" && elem.Attribute("name").Value == "SECRET")
                        return elem.Value;                    
                }

                throw new Exception("A-OK, Check.");
            }
        }
    }
}
