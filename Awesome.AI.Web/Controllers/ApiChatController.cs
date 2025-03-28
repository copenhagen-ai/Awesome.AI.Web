using Awesome.AI.Web.Common;
using Awesome.AI.Web.Helpers;
using Awesome.AI.Web.Hubs;
using Microsoft.AspNetCore.Mvc;
using static Awesome.AI.Variables.Enums;

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

        private static bool busy { get; set; }
        
        // POST api/<ApiChatController>
        [HttpPost]
        public async Task<PostResponce> Post([FromBody] Post obj)
        {
            bool reset = true;
            try
            {
                if (busy) {

                    string str = obj.text.Length > 45 ? obj.text[..45] : obj.text;

                    RoomHub.ResetAsked();
                    MINDS m = obj.mind.ToUpper() == MINDS.ROBERTA.ToString() ? MINDS.ROBERTA : MINDS.ANDREW;
                    ChatComm.Add(m, $">> user:{str}..<br>");
                    string _res = ChatComm.GetResponce(m);
                    reset = false;
                    return new PostResponce() { ok = false, res = $"{_res}" };
                }

                busy = true;

                MINDS _m = obj.mind.ToUpper() == MINDS.ROBERTA.ToString() ? MINDS.ROBERTA : MINDS.ANDREW;
                Instance inst = _m == MINDS.ROBERTA ? RoomHub.Instances[0] : RoomHub.Instances[1];
                string question = obj.text;

                if (question.Length > 45) {
                    RoomHub.ResetAsked();
                    string str = "question is too long..";
                    ChatComm.Add(_m, $">> user:{question[..45]}..<br>");
                    ChatComm.Add(_m, $">> ass:{str}<br>");
                    string _res = ChatComm.GetResponce(_m);
                    busy = false;
                    return new PostResponce() { ok = true, res = $"{_res}" };
                }

                if (question == "") {
                    RoomHub.ResetAsked();
                    string str = ". . .";
                    ChatComm.Add(_m, $">> ass:{str}<br>");
                    string _res = ChatComm.GetResponce(_m);                    
                    return new PostResponce() { ok = true, res = $"{_res}" };
                }

                //if (inst.mind.loc.LocationState > 0) {
                //    RoomHub.ResetAsked();
                //    string str = "dont you see im busy..";
                //    ChatComm.Add(_m, $">> user:{question}<br>");
                //    ChatComm.Add(_m, $">> ass:{str}<br>");
                //    string _res = ChatComm.GetResponce(_m);
                //    busy = false;
                //    return new PostResponce() { ok = true, res = $"{_res}" };
                //}

                RoomHelper helper = new RoomHelper();

                string content = helper.GPTGiveMeAnAnswer(_m, question);
            
                if(content == null) {
                    RoomHub.ResetAsked();
                    string str = "my bad..";
                    ChatComm.Add(_m, $">> user:{question}<br>");
                    ChatComm.Add(_m, $">> ass:{str}<br>");
                    string _res = ChatComm.GetResponce(_m);                    
                    return new PostResponce() { ok = true, res = $"{_res}" };
                }

                //inst.mind.chat_answer = true;
                string ans = await inst.mind._out.GetAnswer();
                //inst.mind.chat_answer = false;
                RoomHub.ResetAsked();

                string res = 
                    ans == ":COMEAGAIN" ? 
                    "come again.." : 
                    ans == ":YES" ? 
                    content : 
                    ans;
                
                ChatComm.Add(_m, $">> user:{question}<br>");
                ChatComm.Add(_m, $">> ass:{res}<br>");

                res = ChatComm.GetResponce(_m);

                //busy = false;

                return new PostResponce() { ok = true, res = res };
            }
            catch 
            { 
                RoomHub.ResetAsked();
                MINDS m = obj.mind.ToUpper() == MINDS.ROBERTA.ToString() ? MINDS.ROBERTA : MINDS.ANDREW;
                string _res = ChatComm.GetResponce(m);
                return new PostResponce() { ok = false, res = $"{_res}" };
            }
            finally 
            {
                if(reset)
                    busy = false;                
            }
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
    }
}
