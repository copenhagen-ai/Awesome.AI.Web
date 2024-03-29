﻿using Awesome.AI.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awesome.AI.Web.Api.Users
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
        public string viewers { get; set; }
    }

    public class PostResponce
    {
        public string guid { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsersController : ControllerBase
    {
        
        // GET: api/<ValuesController>
        [HttpGet]
        //[Authorize]
        public GetResponce Get()
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
        }

        // GET api/<ValuesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<ValuesController>
        [HttpPost]
        //[Authorize]
        public PostResponce Post([FromBody] Post obj)
        {
            try
            {
                //string guid = Guid.NewGuid().ToString();

                string guid = obj.guid;

                UserHelper.AddUser(guid);

                PostResponce res = new PostResponce();
                res.guid = guid;
            
                return res;
            }
            catch (Exception _e)
            {
                PostResponce res = new PostResponce();
                res.guid = "1234";

                return res;
            }
        }

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
