using Awesome.AI.Common;
using Awesome.AI.Web.Helpers;
using Awesome.AI.Web.Hubs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awesome.AI.Web.Api.Users
{
    public class Post
    {
        public string value { get; set; }
    }

    public class GetResponce
    {
        public string viewers { get; set; }
        public bool server_running { get; set; }
    }

    public class PostResponce
    {
        public bool ip_is_registered { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ApiServerChecksController : ControllerBase
    {        
        private static int UserCount {  get; set; }

        [HttpGet]
        public GetResponce Get()
        {
            try
            {
                GetResponce res = new GetResponce();
                res.viewers = "" + UserCount;
                res.server_running = RoomHub.is_running;

                return res;
            }
            catch (Exception _e)
            {
                GetResponce res = new GetResponce();
                res.viewers = "-1";
                return res;
            }
        }
                        
        [HttpPost]        
        public PostResponce Post([FromBody] Post obj)
        {
            try
            {
                string val = obj.value;

                if (val.IsNullOrEmpty())
                    return new PostResponce();

                if(val != "new user")
                    return new PostResponce();

                var remoteIp = Request?.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                
                if (remoteIp.IsNullOrEmpty())
                    throw new Exception();

                bool ip_is_registered = UserHelper.CheckIP(remoteIp);
                UserHelper.AddUser(remoteIp);
                UserCount = UserHelper.CountUsers();

                PostResponce res = new PostResponce();
                res.ip_is_registered = ip_is_registered;
                
                return res;
            }
            catch (Exception _e)
            {
                PostResponce res = new PostResponce();
                res.ip_is_registered = true;

                return res;
            }
        }

        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // PUT api/<ValuesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] Put obj)
        //{
        //}

        // DELETE api/<ValuesController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
