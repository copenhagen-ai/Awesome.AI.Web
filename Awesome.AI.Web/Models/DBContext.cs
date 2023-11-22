//using Microsoft.EntityFrameworkCore;

//namespace Awesome.AI.Web.Models
//{
//    public class MyDbContext : DbContext
//    {
//        public string ConnectionString { get; set; }

//        public MyDbContext(string connectionString)
//        {
//            this.ConnectionString = connectionString;
//        }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            /*
//             * #warning To protect potentially sensitive information in your connection string, 
//             * you should move it out of source code.See http://go.microsoft.com/fwlink/?LinkId=723263 
//             * for guidance on storing connection strings.
//             * */

//            optionsBuilder.UseMySQL(ConnectionString);
//        }
//    }
//}