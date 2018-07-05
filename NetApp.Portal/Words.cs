using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;

namespace NetApp.Portal
{
    public class Words
    {
        public async void GetWordDefine(string w)
        {
            var handler = new HttpClientHandler() { UseCookies = true };
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:57.0) Gecko/20100101 Firefox/57.0");
                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                HttpResponseMessage response;
                Uri uri = new Uri(@"http://www.zdic.net/sousou/");
                response = await client.GetAsync(uri);
                var cookies = response.Headers.GetValues("Set-Cookie");
                string data = $"lb_a=hp&lb_b=mh&lb_c=mh&tp=tp1&q={HttpUtility.UrlEncode(w)}";
                using (var content = new StringContent(data))
                {
                    try
                    {
                        content.Headers.Add("Cookie", cookies);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                        // Make the REST API call.
                        response = await client.PostAsync(uri, content);
                        if (response != null)
                        {
                            string contentString = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(contentString))
                            {
                                ;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        var m = ex.Message;
                    }
                }
            }
        }
    }
}
