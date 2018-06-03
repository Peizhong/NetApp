using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NetApp.Portal
{
    public class MachineLearning
    {
        private MachineLearning() { }

        public static readonly MachineLearning Instance = new MachineLearning();


        #region Computer Vision
        const string keyForCV = "e0f85ad136764fabb1fedc612f735d26";
        const string urlForCV = @"https://westeurope.api.cognitive.microsoft.com/vision/v1.0/analyze";

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (var fs = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fs);
                return binaryReader.ReadBytes((int)fs.Length);
            }
        }

        async Task<string> RequestComputerVisionAsync(string imgPath)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", keyForCV);
                string requestParameters = "visualFeatures=Categories,Description,Color&language=en";
                string uri = $"{urlForCV}?{requestParameters}";
                var data = new Dictionary<string, string>()
                {
                    ["url"] = imgPath
                };
                var json = JsonConvert.SerializeObject(data);
                HttpResponseMessage response;
                using (var content = new ByteArrayContent(Encoding.UTF8.GetBytes(json)))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }
                /*
                byte[] byteData = GetImageAsByteArray(imgPath);
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }*/
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(response.ReasonPhrase);
                }
                string contentString = await response.Content.ReadAsStringAsync();
                return contentString;
            }
        }

        public async Task<string> GetComputerVisionResult(string imgPath)
        {
            string result = await RequestComputerVisionAsync(imgPath);
            return result;
        }
        #endregion
    }
}