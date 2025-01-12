using Awesome.AI.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awesome.AI.Web.Api.Error
{
    public class Post
    {
        public string guid { get; set; }
    }

    //public class Put
    //{
    //    public string value { get; set; }
    //}

    public class GetError
    {
        public string error { get; set; }
    }

    public class GetMessage
    {
        public string message { get; set; }
    }

    public class GetPath
    {
        public string path { get; set; }
    }

    public class PostResponce
    {
        public string guid { get; set; }
    }

    [ApiController]
    public class ApiErrorController : ControllerBase
    {

        // GET: api/<ValuesController>
        [HttpGet]
        [Route("api/error")]
        [Authorize]
        public GetError Error()
        {
            try
            {
                //return new GetResponce() { error = PathHelper.PathSetup };

                string error = XmlHelper.GetError(); ;

                GetError res = new GetError();
                res.error = error;

                return res;
            }
            catch (Exception _e)
            {
                return new GetError() { error = _e.Message };
            }
        }

        // GET: api/<ValuesController>
        [HttpGet]
        [Route("api/message")]
        [Authorize]
        public GetMessage Message()
        {
            try
            {
                //return new GetResponce() { error = PathHelper.PathSetup };

                string msg = XmlHelper.GetMessage(); ;

                GetMessage res = new GetMessage();
                res.message = msg;

                return res;
            }
            catch (Exception _e)
            {
                return new GetMessage() { message = _e.Message };
            }
        }

        // GET: api/<ValuesController>
        //[HttpGet]
        //[Route("api/path")]
        //[Authorize]
        //public GetPath Path()
        //{
        //    try
        //    {
        //        //return new GetResponce() { error = PathHelper.PathSetup };

        //        string path = PathSetup.MyPath("andrew");

        //        GetPath res = new GetPath();
        //        res.path = path;

        //        return res;
        //    }
        //    catch (Exception _e)
        //    {
        //        return new GetPath() { path = _e.Message };
        //    }
        //}

        // GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<ValuesController>
        //[HttpPost]
        //public PostResponce Post([FromBody] Post obj)
        //{
        //    //string guid = Guid.NewGuid().ToString();

        //    string guid = obj.guid;

        //    StaticsHelper.AddUser(guid);

        //    PostResponce res = new PostResponce();
        //    res.guid = guid;

        //    return res;
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
