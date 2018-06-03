using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace NetApp.Portal
{
    public class MachineLearning
    {
        private MachineLearning() { }

        public static readonly MachineLearning Instance = new MachineLearning();


        #region Computer Vision
        const string keyForCV = "e0f85ad136764fabb1fedc612f735d26";
        const string urlForCV = @"https://westeurope.api.cognitive.microsoft.com/vision/v1.0/anaylze";

        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        async Task<string> RequestComputerVisionAsync(string imgPath)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", keyForCV);
                string requestParameters = "visualFeatures=Categories";
                string uri = $"{urlForCV}?{requestParameters}";
                HttpResponseMessage response;
                byte[] byteData = GetImageAsByteArray(imgPath);
                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception(response.ReasonPhrase);
                }
                string contentString = await response.Content.ReadAsStringAsync();
                return contentString;
            }
        }

        public async Task<string> GetComputerVisionResult(string imgPath)
        {
            if (!File.Exists(imgPath))
                return string.Empty;

            string result = await RequestComputerVisionAsync(imgPath);
            return result;
        }
        #endregion
    }
}
