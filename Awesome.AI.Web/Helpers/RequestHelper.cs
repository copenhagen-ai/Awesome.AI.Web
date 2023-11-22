namespace Awesome.AI.Web.Helpers
{
    public class RequestHelpers
    {
        public static string GetCanonical(HttpContext request)
        {
            string sch = request.Request.Scheme;
            string host = request.Request.Headers["host"];
            string path = request.Request.Path;
            
            string res = string.Format("{0}://{1}{2}", sch, host, path);
            if(res.ElementAt(res.Length - 1) == '/')
                res = res.Substring(0, res.Length - 1);

            return res;
        }
    }
}
