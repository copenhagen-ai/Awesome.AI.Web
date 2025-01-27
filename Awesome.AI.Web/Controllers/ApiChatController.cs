using Awesome.AI.Web.Common;
using Awesome.AI.Web.Helpers;
using Awesome.AI.Web.Hubs;
using Awesome.AI.Web.Models;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Relational;
using System.Diagnostics;
using System.Text.Json;
using System.Web;
using static Awesome.AI.Helpers.Enums;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Awesome.AI.Web.Controllers
{
    public class Post
    {
        public string text { get; set; }
        public string mind { get; set; }
    }

    public class PostResponce
    {
        public string res { get; set; }
        public bool ok { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ApiChatController : ControllerBase
    {
        private int counter = 0;

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

        private static Stopwatch watch { get; set; }
        // POST api/<ApiChatController>
        [HttpPost]
        public async Task<PostResponce> Post([FromBody] Post obj)
        {
            counter++;

            string question = obj.text;
            string mind = obj.mind;
            MINDS _m = mind.ToUpper() == MINDS.ROBERTA.ToString() ? MINDS.ROBERTA : MINDS.ANDREW;
            Instance inst = _m == MINDS.ROBERTA ? RoomHub.Instances[0] : RoomHub.Instances[1];

            if(watch != null) { 
                watch.Stop();
                var m_sec = watch.ElapsedMilliseconds;
                watch = System.Diagnostics.Stopwatch.StartNew();

                if (m_sec < 5000) {
                    RoomHub.ResetAsked();// inst.mind.chat_asked = false;
                    string _res = ChatComm.GetResponce(_m);
                    return new PostResponce() { ok = false, res = $"{_res}" };
                }                
            }

            watch = System.Diagnostics.Stopwatch.StartNew();        

            if (question.Length > 22) {
                RoomHub.ResetAsked();// inst.mind.chat_asked = false;
                string str = "Question is too long..";
                ChatComm.Add(_m, $">> user:{question[..22]}..<br>");
                ChatComm.Add(_m, $">> ass:{str}<br>");
                string _res = ChatComm.GetResponce(_m);
                return new PostResponce() { ok = true, res = $"{_res}" };
            }

            if (question == "") {
                RoomHub.ResetAsked();// inst.mind.chat_asked = false;
                string str = ". . .";
                ChatComm.Add(_m, $">> ass:{str}<br>");
                string _res = ChatComm.GetResponce(_m);
                return new PostResponce() { ok = true, res = $"{_res}" };
            }

            if (inst.mind.loc.LocationState > 0) {
                RoomHub.ResetAsked();// inst.mind.chat_asked = false;
                string str = "Dont you see Im busy..";
                ChatComm.Add(_m, $">> user:{question}<br>");
                ChatComm.Add(_m, $">> ass:{str}<br>");
                string _res = ChatComm.GetResponce(_m);
                return new PostResponce() { ok = true, res = $"{_res}" };
            }

            string _json = Json(_m, question);
            string _base = "https://api.openai.com";
            string _path = "v1/chat/completions";
            string _params = "";
            string _accept = "";
            string _contenttype = "application/json";
            string _apikey = "";
            string _token = SettingsHelper.SECRET;
            string _secret = "";
            string gpt = RestHelper.Send(HttpMethod.Post, _json, _base, _path, _params, _accept, _contenttype, _apikey, _token, _secret);

            if(gpt == null) {
                RoomHub.ResetAsked();// inst.mind.chat_asked = false;
                string str = "My bad..";
                ChatComm.Add(_m, $">> user:{question}<br>");
                ChatComm.Add(_m, $">> ass:{str}<br>");
                string _res = ChatComm.GetResponce(_m);
                return new PostResponce() { ok = true, res = $"{_res}" };
            }

            Root root = JsonSerializer.Deserialize<Root>(gpt);
            string content = root.choices[0].message.content;
            content = content.Length > 85 ? $"{content[..85]}..." : content;

            inst.mind.chat_answer = true;
            string ans = await inst.mind._out.GetAnswer();
            inst.mind.chat_answer = false;
            RoomHub.ResetAsked();

            string res = ans == ":YES" ? content : ans;

            ChatComm.Add(_m, $">> user:{question}<br>");
            ChatComm.Add(_m, $">> ass:{res}<br>");

            res = ChatComm.GetResponce(_m);

            Stopper();

            return new PostResponce() { ok = true, res = res };
        }

        private async Task Stopper()
        {
            await Task.Delay(10000);

            if (watch == null)
                return;
            
            watch.Stop();
            watch = null;
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

        private string Json(MINDS mind, string txt)
        {
            string str = ChatComm.GetResponce(mind);

            str = str.Replace("<br>", "");

            List<string> conv = str.Split(">> ").ToList();

            string json = "{" +
                "\"model\": \"gpt-3.5-turbo\"," +
                "\"messages\": [" +
                "{\"role\": \"system\", \"content\": \"you are a happy assistant\"},";
                
            foreach (string s in conv)
            {
                if (s.StartsWith("user")) {
                    string tmp = s.Replace("user:", "");
                    json += "{\"role\": \"user\", \"content\": \"" + tmp + "\"},";
                }
                else if (s.StartsWith("ass")) {
                    string tmp = s.Replace("ass:", "");
                    json += "{\"role\": \"assistant\", \"content\": \"" + tmp + "\"},";
                }
            }

            json += "{\"role\": \"user\", \"content\": \""+ txt + ". (answer in 10 words or less)\"}";

            json += "],\"temperature\": 0.7}";

            return json;
        }
    }
}
