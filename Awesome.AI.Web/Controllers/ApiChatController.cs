using Awesome.AI.Web.Helpers;
using Awesome.AI.Web.Hubs;
using Awesome.AI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Relational;
using System.Text.Json;
using System.Web;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awesome.AI.Web.Controllers
{
    public class Post
    {
        public string text { get; set; }
    }

    public class PostResponce
    {
        public string res { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ApiChatController : ControllerBase
    {
        private int counter = 0;
        private static List<string> communication {  get; set; }

        // GET: api/<ApiChatController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<ApiChatController>/5
        //[HttpGet("{id}")]
        //public GetResponce Get(int id)
        //{
        //    counter++;
        //    return new GetResponce() { res = "value: " + counter };
        //}

        // POST api/<ApiChatController>
        [HttpPost]
        public async Task<PostResponce> Post([FromBody] Post obj)
        {
            counter++;

            if(communication == null)
                communication = new List<string>();
            
            string question = obj.text;
            Instance inst= RoomHub.Instances[0];

            if(question.Length > 22)
                return new PostResponce() { res = "question too long.." };

            string _json = Json(question);
            string _base = "https://api.openai.com";
            string _path = "v1/chat/completions";
            string _params = "";
            string _accept = "";
            string _contenttype = "application/json";
            string _apikey = "";
            string _token = SettingsHelper.SECRET;
            string _secret = "";
            string gpt = RestHelper.Send(HttpMethod.Post, _json, _base, _path, _params, _accept, _contenttype, _apikey, _token, _secret);

            if(gpt == null)
                return new PostResponce() { res = "processing.." };

            Root root = JsonSerializer.Deserialize<Root>(gpt);
            string content = root.choices[0].message.content;
            content = content.Length > 65 ? $"{content[..65]}..." : content;



            inst.mind.process_answer = true;
            string ans = await inst.mind._out.GetAnswer();
            inst.mind.process_answer = false;

            string res = ans == ":YES" ? content : ans;

            communication.Add($">> {question}<br>");
            communication.Add($">> {res}<br>");

            if(communication.Count >= 6)
            {
                communication.RemoveAt(0);
                communication.RemoveAt(0);
            }

            res = "";
            foreach(string str in communication)
                res += str;

            return new PostResponce() { res = res };
        }

        // PUT api/<ApiChatController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ApiChatController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        private string Json(string txt)
        {
            string json = "{" +
                "\"model\": \"gpt-3.5-turbo\"," +
                "\"messages\": [" +
                "{\"role\": \"system\", \"content\": \"you are a happy assistant\"}," +
                "{\"role\": \"user\", \"content\": \"" + txt +
                "\"}]," +
                "\"temperature\": 0.7" +
                "}";

            return json;
        }
    }
}
