using Awesome.AI.Common;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Awesome.AI.Web.Helpers
{
    public class RestHelper
    {
        public static string Send(HttpMethod method,
            string _json,
            string _base,
            string _path,
            string _params,
            string _accept,
            string _contenttype,
            string _apikey,
            string _token,
            string _secret
            )
        {
            try
            {
                using (var client = new HttpClient())
                {
                    _params = _params != "" ? "?" + _params : "";

                    _apikey = _params == "" && _apikey != "" ? "?api_key=" + _apikey :
                              _params != "" && _apikey != "" ? "&api_key=" + _apikey : "";

                    client.BaseAddress = new Uri(_base);
                    HttpRequestMessage req = new HttpRequestMessage(method, "/" + _path + _params + _apikey);

                    if (_json != "")
                        req.Content = new StringContent(_json, Encoding.UTF8, _contenttype);

                    if (_accept != "")
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_accept));//ACCEPT header application/x-www-form-urlencoded

                    if (_token != "")
                        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

                    if (_contenttype != "")
                        req.Content.Headers.ContentType = new MediaTypeHeaderValue(_contenttype);

                    if (_secret != "")
                        req.Headers.Add("Authorization", _secret);

                    HttpResponseMessage res = client.SendAsync(req).Result;
                    if (res.IsNull())
                        return null;

                    if (!res.IsSuccessStatusCode)
                        return null;

                    string _res = res.Content.ReadAsStringAsync().Result;
                    return _res;
                }
            }
            catch (Exception _e)
            {
                return null;
            }
        }
    }
}