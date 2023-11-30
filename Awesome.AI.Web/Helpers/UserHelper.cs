using Awesome.AI.Common;
using Awesome.AI.Web.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Awesome.AI.Web.Helpers
{
    public class User
    {
        public string Guid { get; set; }
        public DateTime Time { get; set; }
    }

    public class UserHelper
    {
        public static List<User> Users { get; set; }

        public static void AddUser(string guid)
        {
            if (Users.IsNullOrEmpty())
                Users = new List<User>();

            User _u = Users.Where(x => x.Guid == guid).FirstOrDefault();
            if (!_u.IsNull())
                Users.Remove(_u);

            Users.Add(new User() { Guid = guid, Time = DateTime.Now });
        }

        public static void UpdateUsers()
        {
            if (Users.IsNullOrEmpty())
                Users = new List<User>();

            DateTime now = DateTime.Now.AddMinutes(-2);
            Users = Users.Where(x => x.Time > now).ToList();                    
        }

        public static int CountUsers() 
        {
            if (Users.IsNullOrEmpty())
                Users = new List<User>();

            return Users.Count();
        }

        public async static void MaintainUsers()
        {
            try
            {
                XmlHelper.WriteMessage("maintain users..");
                int time = 1000 * 60 * 1 + 1000;
            
                await Task.Delay(1000);
                while (true)
                {
                    UpdateUsers();

                    await Task.Delay(time);
                }
            }
            catch (Exception _e)
            {
                XmlHelper.WriteError("maintainUsers - " + _e.Message);

                MaintainUsers();
            }
        }        
    }
}
