using Awesome.AI.Common;
using Awesome.AI.Web.Helpers;
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
    }

    public class PostResponce
    {
        public string ok { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsersController : ControllerBase
    {        
        private static int UserCount {  get; set; }

        [HttpGet]
        public GetResponce Get()
        {
            try
            {
                GetResponce res = new GetResponce();
                res.viewers = "" + UserCount;

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
                    return new PostResponce();

                UserHelper.AddUser(remoteIp);
                UserCount = UserHelper.CountUsers();

                PostResponce res = new PostResponce();
                res.ok = "ok";
            
                return res;
            }
            catch (Exception _e)
            {
                PostResponce res = new PostResponce();
                res.ok = "not ok";

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
