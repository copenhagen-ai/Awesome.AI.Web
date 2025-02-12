using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awesome.AI.Web.Api.AI
{
    public class Post
    {
        public string guid { get; set; }
    }

    //public class Put
    //{
    //    public string value { get; set; }
    //}

    public class GetResponce
    {
        public string res { get; set; }
    }

    public class PostResponce
    {
        public string guid { get; set; }
    }

    [ApiController]
    public class ApiAIController : ControllerBase
    {
        
        // GET: api/<ValuesController>
        //[HttpGet]
        //[Authorize]
        /*public GetResponce Get()
        {
            try
            {
                string viewers = "" + UserHelper.CountUsers();

                GetResponce res = new GetResponce();
                res.viewers = viewers;

                return res;
            }
            catch (Exception _e)
            {
                GetResponce res = new GetResponce();
                res.viewers = "-1";
                return res;
            }
        }*/

        // GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<ValuesController>
        //[HttpGet]
        //[Route("api/sort")]
        ////[Authorize]
        //public GetResponce Sort()
        //{
        //    try
        //    {
        //        //RoomHub.is_index = !RoomHub.is_index;

        //        return new GetResponce() { res = "ok" };
        //    }
        //    catch (Exception _e)
        //    {
        //        return new GetResponce() { res = "error" };
        //    }
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
