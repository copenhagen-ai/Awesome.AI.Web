namespace Awesome.AI.Web.Helpers
{
    public class User
    {
        public string Ip { get; set; }
        public DateTime Time { get; set; }
    }

    public class UserHelper
    {
        public static List<User> Users { get; set; }

        public static void AddUser(string ip)
        {
            Users ??= new List<User>();

            DateTime now = DateTime.Now;
            DateTime then = DateTime.Now.AddHours(-24);

            Users.Add(new User() { Ip = ip, Time = now });
            Users = Users.Where(x => x.Time > then).ToList();
        }

        public static int CountUsers() 
        {
            Users ??= new List<User>();

            int count = Users.Select(x=>x.Ip).Distinct().Count();

            return count;
        }

        //private static bool IsRunning {  get; set; }
        //public async static void MaintainUsers()
        //{
        //    try
        //    {
        //        if (IsRunning)
        //            return;

        //        IsRunning = true;

        //        Users ??= new List<User>();

        //        XmlHelper.WriteMessage("maintain users..");
                
        //        int time = 1000 * 60 * 1;
            
        //        await Task.Delay(800);
                
        //        while (true)
        //        {
        //            DateTime now = DateTime.Now.AddHours(-24);

        //            Users = Users.Where(x => x.Time > now).ToList();

        //            await Task.Delay(time);
        //        }
        //    }
        //    catch (Exception _e)
        //    {
        //        XmlHelper.WriteError("maintainUsers - " + _e.Message);

        //        IsRunning = false;

        //        MaintainUsers();
        //    }
        //}
    }
}
