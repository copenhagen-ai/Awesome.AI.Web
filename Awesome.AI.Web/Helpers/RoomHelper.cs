using Awesome.AI.Common;
using Awesome.AI.Helpers;
using Awesome.AI.Web.Common;
using Awesome.AI.Web.Hubs;
using Awesome.AI.Web.Models;
using Org.BouncyCastle.Asn1.X509;
using System.Text.Json;
using static Awesome.AI.Helpers.Enums;

namespace Awesome.AI.Web.Helpers
{
    public class RoomHelper
    {
        //public long Remaining(Instance inst, bool is_running)
        //{
        //    if(!is_running)
        //        return 1;

        //    int ms_wait = inst.sec_message * 1000;
        //    long remainingSec = (ms_wait - inst.elapsedMs) / 1000;

        //    return remainingSec;
        //}

        //public bool Active(bool even, bool all)
        //{
        //    if(!all)
        //        return true;

        //    bool is_active = DateTime.Now.Hour % 2 == 0;

        //    return even ? is_active : !is_active;
        //}

        //public int Delay(Instance inst, int when_active, int when_inactive)
        //{
        //    int sec_delay = inst.is_active ? when_active : when_inactive;

        //    if (inst.fast_responce)
        //        sec_delay = 1 * 1;
            
        //    return sec_delay;
        //}

        //private string FastResponce(ref bool fast_responce)
        //{
        //    fast_responce = true;
        //    return old_message;
        //}

        private string old_message { get; set; }
        public string GPTConnectTheDots(string dot1, string dot2/*, ref bool fast_responce*/)
        {
            //fast_responce = false;

            if (string.IsNullOrEmpty(old_message))
                old_message = "";

            if (dot1 == dot2)
                return old_message;// return FastResponce(ref fast_responce);

            string _json = Json1(dot1, dot2);
            string _base = "https://api.openai.com";
            string _path = "v1/chat/completions";
            string _params = "";
            string _accept = "";
            string _contenttype = "application/json";
            string _apikey = "";
            string _token = SettingsHelper.SECRET;
            string _secret = "";
            string gpt = RestHelper.Send(HttpMethod.Post, _json, _base, _path, _params, _accept, _contenttype, _apikey, _token, _secret);

            if (gpt.IsNull())
                return old_message;// return FastResponce(ref fast_responce);

            Root root = JsonSerializer.Deserialize<Root>(gpt);
            string content = root.choices[0].message.content;
            content = Format1(content);
            content = Format2(content);
            content = Format3(content);

            if (Invalid(content))
                return old_message;// return FastResponce(ref fast_responce);

            old_message = content;

            return content;
        }

        public string GPTGiveMeADot(Instance inst, UNIT common)
        {
            string subject = common.HUB.subject;

            if (Constants.subject_decision.Contains(subject))
                return "";

            string str = "" + common.Index;
            int dot = str.IndexOf(',');
            string index = str.Substring(0, dot + 2);

            string _json = Json2(inst, subject, index);
            string _base = "https://api.openai.com";
            string _path = "v1/chat/completions";
            string _params = "";
            string _accept = "";
            string _contenttype = "application/json";
            string _apikey = "";
            string _token = SettingsHelper.SECRET;
            string _secret = "";
            string gpt = RestHelper.Send(HttpMethod.Post, _json, _base, _path, _params, _accept, _contenttype, _apikey, _token, _secret);

            if (gpt.IsNull())
                return null;

            Root root = JsonSerializer.Deserialize<Root>(gpt);
            string content = root.choices[0].message.content;
            content = Format1(content);

            return content;
        }

        public string GPTAskMeAQuestion(Instance inst, string sub)
        {
            //string str = "" + dex;
            //int dot = str.IndexOf(',');
            //string index = str.Substring(0, dot);

            string _json = Json3(inst, sub);
            string _base = "https://api.openai.com";
            string _path = "v1/chat/completions";
            string _params = "";
            string _accept = "";
            string _contenttype = "application/json";
            string _apikey = "";
            string _token = SettingsHelper.SECRET;
            string _secret = "";
            string gpt = RestHelper.Send(HttpMethod.Post, _json, _base, _path, _params, _accept, _contenttype, _apikey, _token, _secret);

            if (gpt.IsNull())
                return null;

            Root root = JsonSerializer.Deserialize<Root>(gpt);
            string content = root.choices[0].message.content;
            content = Format1(content);

            return content;
        }

        public string GPTGiveMeAnAnswer(MINDS _m, string question)
        {
            string _json = Json4(_m, question);
            string _base = "https://api.openai.com";
            string _path = "v1/chat/completions";
            string _params = "";
            string _accept = "";
            string _contenttype = "application/json";
            string _apikey = "";
            string _token = SettingsHelper.SECRET;
            string _secret = "";
            string gpt = RestHelper.Send(HttpMethod.Post, _json, _base, _path, _params, _accept, _contenttype, _apikey, _token, _secret);

            if (gpt == null)
                return null;

            Root root = JsonSerializer.Deserialize<Root>(gpt);
            string content = root.choices[0].message.content;
            content = content.Length > 85 ? $"{content[..85]}..." : content;

            return content;
        }

        public string Format1(string json)
        {
            json = json.ToLower();

            json = json.Replace("\n          ", "");
            json = json.Replace("\n         ", "");
            json = json.Replace("\n        ", "");
            json = json.Replace("\n       ", "");
            json = json.Replace("\n      ", "");
            json = json.Replace("\n     ", "");
            json = json.Replace("\n    ", "");
            json = json.Replace("\n   ", "");
            json = json.Replace("\n  ", "");
            json = json.Replace("\n ", "");
            json = json.Replace("\n", "");

            json = json.Replace("\\n", "");
            json = json.Replace("\"", "");
            json = json.Replace("'", "");
            json = json.Replace(".", "");
            json = json.Replace(",", "");
            json = json.Replace(":", "");
            json = json.Replace(";", "");
            json = json.Replace("(", "");
            json = json.Replace(")", "");
            json = json.Replace("?", "");
            json = json.Replace("!", "");

            return json;
        }

        public string Format2(string json)
        {
            json = json.ToLower();

            json = json.Replace("sure i understand lets give it a try resulting sentence", "");
            json = json.Replace("sure i understand lets begin", "");
            
            json = json.Replace("sure heres a resulting sentence", "");
            json = json.Replace("sure heres the resulting sentence", "");
            json = json.Replace("sure here is the resulting sentence", "");
            json = json.Replace("sure lets play heres my resulting sentence", "");
            json = json.Replace("sure lets play heres the resulting sentence", "");
            json = json.Replace("sure lets play heres your resulting sentence", "");
            json = json.Replace("sure lets play here is the resulting sentence", "");
            json = json.Replace("sure lets play the game heres the resulting sentence", "");
            json = json.Replace("sure lets play the game here is the resulting sentence", "");
            json = json.Replace("sure lets give it a try heres the resulting sentence", "");
            json = json.Replace("sure i can play that game heres the resulting sentence", "");
            json = json.Replace("sure i would be happy to play the game with you heres the resulting sentence", "");
            json = json.Replace("sure im ready to play heres the resulting sentence", "");
            json = json.Replace("sure im excited to play heres the resulting sentence", "");
            json = json.Replace("sure im excited to play this game with you heres the resulting sentence", "");
            json = json.Replace("sure im happy to play the game heres the resulting sentence", "");
            json = json.Replace("sure im happy to play the game with you heres the resulting sentence", "");
            json = json.Replace("sure im happy to play this game with you heres the resulting sentence", "");
            json = json.Replace("sure im happy to play this game with you here is the resulting sentence", "");
            json = json.Replace("sure im happy to play heres the resulting sentence", "");
            json = json.Replace("sure im happy to play along heres the resulting sentence", "");
            json = json.Replace("sure id be happy to play along heres the resulting sentence", "");
            json = json.Replace("sure id be happy to play the game heres the resulting sentence", "");
            json = json.Replace("sure id be happy to play the game with you heres the resulting sentence", "");
            json = json.Replace("sure id be happy to play the game with you heres my resulting sentence", "");
            json = json.Replace("sure id be happy to play the game with you here is the resulting sentence", "");
            json = json.Replace("sure id be happy to play this game here is the resulting sentence", "");
            json = json.Replace("sure id be happy to play this game with you heres the resulting sentence", "");
            json = json.Replace("sure id love to play this game with you heres the resulting sentence", "");
            json = json.Replace("sure ill be happy to play the game with you heres the resulting sentence", "");
            json = json.Replace("sure ill give it a try heres the resulting sentence", "");
            json = json.Replace("sure ill play along heres the resulting sentence", "");
            json = json.Replace("ill do my best heres the resulting sentence", "");
            json = json.Replace("im excited to play this game with you heres the resulting sentence", "");
            json = json.Replace("i understand the game heres the resulting sentence", "");
            json = json.Replace("i would love to play the game with you heres the resulting sentence", "");
            json = json.Replace("i would be happy to play the game heres the resulting sentence", "");
            json = json.Replace("okay lets play heres the resulting sentence", "");
            json = json.Replace("the resulting sentence", "");
            
            json = json.Replace("sure lets play this game heres", "");
            json = json.Replace("sure id love to play the game with you here is", "");
            











            return json;
        }

        private string Format3(string json)
        {
            //dot1 optimized for eternitydot2 hal 9000resulting sentence optimized for eternity the powerful hal 9000 awaits
            //using the given guidelines heres an exampledot1 prone to software glitchesdot2 lost my nametagresulting sentence prone to software glitches can sometimes lead to me losing my nametagplease let me know if theres anything else i can assist you with

            bool dot1 = json.Contains("dot1");
            bool dot2 = json.Contains("dot2");
            bool res = json.Contains("resulting sentence");

            if (!(dot1 && dot2 && res))
                return json;

            int index = json.IndexOf("resulting sentence");
            int end_index = index + "resulting sentence".Length;

            json = $"{json[end_index..]}";

            json = json.Replace("please let me know if theres anything else i can assist you with", "");




            return json;
        }


        private bool Invalid(string content)
        {
            bool invalid = false;

            invalid |= content.Contains("im sorry but im unable to generate a resulting sentence");
            invalid |= content.Contains("im sorry but im unable to generate a new resulting sentence");
            invalid |= content.Contains("im sorry but im not able to generate a sentence");
            invalid |= content.Contains("im sorry but im not able to generate a response");
            invalid |= content.Contains("im sorry but im not able to generate a resulting sentence");
            invalid |= content.Contains("im sorry but im not able to generate the resulting sentence");
            invalid |= content.Contains("im sorry but im not able to play the game");
            invalid |= content.Contains("im sorry but i dont understand the game youre suggesting");
            invalid |= content.Contains("im sorry but i dont have the ability to create new sentences");
            invalid |= content.Contains("im sorry but i am not able to participate in the game");
            invalid |= content.Contains("im sorry but i am unable to fulfill your request");
            invalid |= content.Contains("im sorry but i am unable to generate a resulting sentence");
            invalid |= content.Contains("i apologize but i am unable to generate a response");
            invalid |= content.Contains("i apologize but i am unable to follow the guidelines");
            invalid |= content.Contains("i apologize but i am not able to generate a response");
            invalid |= content.Contains("i apologize but im not able to play the game");
            invalid |= content.Contains("i apologize but im not able to generate a response");
            invalid |= content.Contains("i apologize but im not able to generate a resulting sentence");
            invalid |= content.Contains("i apologize but im unable to generate a resulting sentence");
            invalid |= content.Contains("i apologize but im unable to generate a response");
            invalid |= content.Contains("i apologize for the confusion but im unable to generate a resulting sentence");
            invalid |= content.Contains("i apologize for any confusion but as a text - based ai");
            invalid |= content.Contains("i apologize for any confusion but as an ai language model");
            invalid |= content.Contains("i apologize for any confusion but as a text - based ai assistant");

            return invalid;
        }

        private string Json1(string dot1, string dot2)
        {
            string json = "{" +
                "\"model\": \"gpt-3.5-turbo\"," +
                "\"messages\": [" +
                "{\"role\": \"system\", \"content\": \"you are a happy assistant\"}," +
                "{\"role\": \"user\", \"content\": \"" +
                
                "This is a new conversation. " +
                "Here are two words or sentences, I will call them dot1 and dot2. " +
                "The word or sentence in dot1 is '" + dot1 + "'. " +
                "The word or sentence in dot2 is '" + dot2 + "'. " +
                "So now lets play a game that resembles the game 'connect the dots'. " +
                "But instead of dots on a piece of paper, I give you 2 sentences or words and then you connect them by adding new words in between and thus creating a new resulting sentence. " +
                "Here are some guidelines: " +
                "So the formula is like this: resulting sentence = dot1 + your words + dot2. " +
                //"\\nYou should connect dot1 and dot2 by adding new between words them, in order to form a new sentence. " +
                //"\\nThe resulting sentence does not have to make sense, but it must start with dot1 and end with dot2. " +
                "The resulting sentence should not have an ending or conclusion. " +
                "Add only less than 5, but more than 1 words to make the resulting sentence. " +
                "Dont change the words or sentences given in dot1 and dot2. " +
                "You are only allowed to use number and letters, so no special characters. " +
                "Only respond with the resulting sentence. " +

                //"\\nYou are only allowed to use these characters:' 0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ'. " +
                //"\\nUse synonyms and relatable words instead of the actual words given in the sentences. " +
                //"\\nDo not alter the sentence given in FACT. Only add the new word to the end. " +
                //"The word/sentence given in TITLEBBB should be at the end of the resulting sentence. " +
                //"\\nThe resulting sentence must not have an ending or conclusion. " +
                //"\\nFor instance if TITLEBBB is 'banana', then instead of saying 'banana', you could say 'apple' or 'fruit'. " +
                //"\\nDo this by using the words/sentence given in TITLEBBB as inspiration for the added words. " +

                //"\\nThe resulting sentence should be like you are talking to yourself. " +
                //"\\nUse TITLEBBB only as a subject for the resulting sentence. " +
                //"You don't have to use the exact words given in TITLEAAA and TITLEBBB, but rather use them as inspiration for other words to use. " +
                //"Don't not use the word given in TITLEAAA in the resulting sentence. " +
                //"You are not allowed to use the word given in TITLEAAA in the sentence. " +
                "\"}]," +
                "\"temperature\": 0.7" +
                "}";

            return json;
        }

        private string Json2(Instance inst, string subject, string index)
        {
            string json = "{" +
                "\"model\": \"gpt-3.5-turbo\"," +
                "\"messages\": [" +
                "{\"role\": \"system\", \"content\": \"you are a logical assistant\"}," +
                "{\"role\": \"user\", \"content\": \"" +

                (inst.type == MINDS.ROBERTA ?
                "on a a scale from 0 to 100, where 0 is the worst you can think of and 100 is the best you can think of, create a sentence that fits the index " + index + ", on the subject '" + subject + "'. " :
                "on a a scale from 0 to 100, where 100 is the worst you can think of and 0 is the best you can think of, create a sentence that fits the index " + index + ", on the subject '" + subject + "'. ") +

                "use 5 words or less. " +
                "only respond with one sentence. " +
                "dont mention the index. " +

                "\"}]," +
                "\"temperature\": 0.7" +
            "}";

            return json;
        }

        private string Json3(Instance inst, string subject)
        {
            string str = ChatComm.GetResponce(inst.type);

            str = str.Replace("<br>", "");

            List<string> conv = str.Split(">> ").ToList();

            string json = "{" +
                "\"model\": \"gpt-3.5-turbo\"," +
                "\"messages\": [" +
                "{\"role\": \"system\", \"content\": \"you are a happy assistant\"},";

            foreach (string s in conv)
            {
                if (s.StartsWith("user"))
                {
                    string tmp = s.Replace("user:", "");
                    json += "{\"role\": \"user\", \"content\": \"" + tmp + "\"},";
                }
                else if (s.StartsWith("ass"))
                {
                    string tmp = s.Replace("ass:", "");
                    json += "{\"role\": \"assistant\", \"content\": \"" + tmp + "\"},";
                }
            }

            json += "{\"role\": \"user\", \"content\": \"ask me a question related to '" + subject + "'. " +
                "use 10 words or less. " +
                "only respond with one sentence. " +                

                "\"}]," +
                "\"temperature\": 0.7" +
                "}";

            return json;
        }

        private string Json4(MINDS mind, string txt)
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
                if (s.StartsWith("user"))
                {
                    string tmp = s.Replace("user:", "");
                    json += "{\"role\": \"user\", \"content\": \"" + tmp + "\"},";
                }
                else if (s.StartsWith("ass"))
                {
                    string tmp = s.Replace("ass:", "");
                    json += "{\"role\": \"assistant\", \"content\": \"" + tmp + "\"},";
                }
            }

            json += "{\"role\": \"user\", \"content\": \"" + txt + ". (answer in 10 words or less)\"}";

            json += "],\"temperature\": 0.7}";

            return json;
        }

        //private string _Json2(string subject, string index)
        //{


        //    string json = "{" +
        //        "\"model\": \"gpt-3.5-turbo\"," +
        //        "\"messages\": [" +
        //        "{\"role\": \"system\", \"content\": \"you are a logical assistant\"}," +
        //        "{\"role\": \"user\", \"content\": \"" +

        //        //"This is a new conversation. " +
        //        "\\ni give you this subject: " + subject + ". " +
        //        "\\ni give you this index: " + index + ". " +
        //        //"\\nthe worst thing to say would have index 0.0 and the best would have index 100.0?. " +
        //        "\\nthe index can be anywhere between zero and a hundred. " +
        //        "\\nuse the index to create a sentence that has a more positive tone when the index is near a hundred and a more negative tone when the index is closer to zero. " +
        //        "\\nuse 3 or less words. " +
        //        "\\nonly respond with one sentence. " +
        //        "\\ndont mention the index. " +

        //        "\"}]," +
        //        "\"temperature\": 0.7" +
        //        "}";

        //    return json;
        //}

    }
}
