using Awesome.AI.Common;

namespace Awesome.AI.Web.Helpers
{
    public class StaticsHelper
    {
        public static Dictionary<string, DateTime> Users { get; set; }

        public static void AddUser(string guid)
        {
            if(Users.IsNullOrEmpty())
                Users = new Dictionary<string, DateTime>();

            if (Users.Where(x=>x.Key == guid).Count() > 0)
                Users.Remove(guid);

            Users.Add(guid, DateTime.Now);
        }

        public static void UpdateUsers()
        {
            if (Users.IsNullOrEmpty())
                Users = new Dictionary<string, DateTime>();

            foreach (var user in Users)
            {
                DateTime now = DateTime.Now;
                if(user.Value < now.AddMinutes(-2))
                    Users.Remove(user.Key);

                if (user.Value > now)
                    break;
            }
        }

        public static int CountUsers() 
        {
            if (Users.IsNullOrEmpty())
                Users = new Dictionary<string, DateTime>();

            return Users.Count();
        }

        public async static void MaintainUsers()
        {
            int time = 1000 * 60 * 1 + 1000;
            
            while (true)
            {
                await Task.Delay(1000);

                UpdateUsers();

                await Task.Delay(time);
            }
        }
    }
}
