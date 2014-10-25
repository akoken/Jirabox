using Jirabox.Core.Contracts;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jirabox
{
    public class HttpManager : IHttpManager
    {
        public async Task<HttpResponseMessage> GetAsync(string url, bool withBasicAuthentication = false, string username = null, string password = null, CancellationTokenSource cancellationTokenSource = null)
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
            try
            {
                if (cancellationTokenSource != null)
                    response = await httpClient.GetAsync(new Uri(url, UriKind.RelativeOrAbsolute), cancellationTokenSource.Token);
                else
                    response = await httpClient.GetAsync(new Uri(url, UriKind.RelativeOrAbsolute));
            }
            catch (TaskCanceledException)
            {
                if (Debugger.IsAttached)
                    Debug.WriteLine("Http request canceled.");
            }
            return response;
        }

        public async Task<HttpResponseMessage> DownloadAttachment(string fileUrl, bool withBasicAuthentication = false, string username = null, string password = null, CancellationTokenSource cancellationTokenSource = null)
        {          
            var httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });
          
            httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");

            if (withBasicAuthentication)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + GetBasicCredentials(username, password));
            }

            HttpResponseMessage response = null;
            try
            {
                if (cancellationTokenSource != null)
                    response = await httpClient.GetAsync(new Uri(fileUrl, UriKind.RelativeOrAbsolute), HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token);
                else
                    response = await httpClient.GetAsync(new Uri(fileUrl, UriKind.RelativeOrAbsolute));
            }
            catch (TaskCanceledException)
            {
                if (Debugger.IsAttached)
                    Debug.WriteLine("Http request canceled.");
            }
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, string data, bool withBasicAuthentication = false, string username = null, string password = null, CancellationTokenSource cancellationTokenSource = null)
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
            try
            {
                if (cancellationTokenSource != null)
                    response = await httpClient.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"), cancellationTokenSource.Token);
                else
                    response = await httpClient.PostAsync(url, new StringContent(data, Encoding.UTF8, "application/json"));
            }
            catch (TaskCanceledException)
            {
                if (Debugger.IsAttached)
                    Debug.WriteLine("Http request canceled.");
            }            
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
