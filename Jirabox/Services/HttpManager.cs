using Jirabox.Core.Contracts;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jirabox
{
    public class HttpManager : IHttpManager
    {
        public async Task<HttpResponseMessage> GetAsync(string url, bool withBasicAuthentication = false, string username = null, string password = null)
        {
            var httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
            if (withBasicAuthentication)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + GetBasicCredentials(username, password));
            }

            HttpResponseMessage response = null;
            response = await httpClient.GetAsync(new Uri(url, UriKind.RelativeOrAbsolute));

            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, string data, bool withBasicAuthentication = false, string username = null, string password = null)
        {
            var httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
            if (withBasicAuthentication)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + GetBasicCredentials(username, password));
            }

            HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));

            return response;
        }

        private string GetBasicCredentials(string username, string password)
        {
            string mergedCredentials = string.Format("{0}:{1}", username, password);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }   
    }
}
