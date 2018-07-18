using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NetApp.Play.Book
{
    /*
     * networking
     */
    class Chap23
    {
        const string HOST = "localhost";
        public async Task GetBaiduAsync()
        {
            //client等待20秒才释放socket，可以重用，不一定要Dispose
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:52.0) Gecko/20100101 Firefox/52.0");
                var response = await client.GetAsync("http://10.10.5.5:8022/web/lcam/fwms/");
                response.EnsureSuccessStatusCode();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var test = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"get {test}");
                }
            }
        }

        public async Task TcpHttpClient()
        {
            int ReadBufferSize = 1024;
           
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(HOST, 4343);
                NetworkStream stream = client.GetStream();
                string header = "GET / HTTP/1.1\r\n" +
                    $"Host: {HOST}:80\r\n" +
                    "Connection: close\r\n" +
                    "\r\n";
                byte[] buffer = Encoding.UTF8.GetBytes(header);
                await stream.WriteAsync(buffer, 0, buffer.Length);
                await stream.FlushAsync();
                var ms = new MemoryStream();
                buffer = new byte[ReadBufferSize];
                int read = 0;
                do
                {
                    read = await stream.ReadAsync(buffer, 0, ReadBufferSize);
                    ms.Write(buffer, 0, read);
                    Array.Clear(buffer, 0, read);
                } while (read > 0);
                ms.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(ms);
                var res = await reader.ReadToEndAsync();
            }
        }
    }
}
