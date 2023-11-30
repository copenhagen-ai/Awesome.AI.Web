using Awesome.AI.Web.copenhagenai;
using System.Xml;

namespace Awesome.AI.Web.Helpers
{
    public class XmlHelper
    {
        private static XmlDocument doc_error = new XmlDocument();
        private static XmlDocument doc_msg = new XmlDocument();

        public static void LoadError()
        {
            doc_error = null;
            doc_error = new XmlDocument();
            doc_error.Load(PathHelper.PathError);
        }

        public static void LoadMessage()
        {
            doc_msg = null;
            doc_msg = new XmlDocument();
            doc_msg.Load(PathHelper.PathMessage);
        }

        public static void ResetError(string msg)
        {
            XmlDocument root = new XmlDocument();

            root.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><error></error>");

            XmlNode foo = root.ChildNodes[1];
            foo.InnerText = msg;
            //root.DocumentElement.AppendChild(foo);

            root.Save(PathHelper.PathError);

            root = null;
        }

        public static void ResetMessage(string msg)
        {
            XmlDocument root = new XmlDocument();

            root.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?><error></error>");

            XmlNode foo = root.ChildNodes[1];
            foo.InnerText = msg;
            //root.DocumentElement.AppendChild(foo);

            root.Save(PathHelper.PathMessage);

            root = null;
        }

        public static void WriteError(string msg)
        {
            try
            {
                LoadError();

                XmlNode root = doc_error.ChildNodes[1];
                root.InnerText = "error: " + msg;
                doc_error.Save(PathHelper.PathError);
            }
            catch (Exception _e)
            {
                ResetError(msg);
            }
        }

        public static void WriteMessage(string msg)
        {
            try
            {
                //throw new Exception();

                LoadMessage();

                XmlNode root = doc_msg.ChildNodes[1];
                root.InnerText = "message: " + msg;
                doc_msg.Save(PathHelper.PathMessage);
            }
            catch (Exception _e)
            {
                ResetMessage(msg);
            }
        }

        public static string GetError()
        {
            try
            {
                LoadError();

                XmlNode root = doc_error.ChildNodes[1];

                return root.OuterXml;
            }
            catch (Exception _e)
            {
                return "xml corrupt";
            }
        }

        public static string GetMessage()
        {
            try
            {
                LoadMessage();

                XmlNode root = doc_msg.ChildNodes[1];

                return root.OuterXml;
            }
            catch (Exception _e)
            {
                return "xml corrupt";
            }
        }
    }
}
