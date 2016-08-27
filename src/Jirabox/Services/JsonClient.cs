using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Jirabox.Core;
using Jirabox.Core.Contracts;
using Newtonsoft.Json;

namespace Jirabox.Services
{
    public class JsonHttpClient : IJsonHttpClient
    {    
        public async Task<TR> Post<T, TR>(Uri requestUri, T data, string username, string password, HttpStatusCode expectedStatus, CancellationTokenSource cancellationTokenSource)
        {
            var serializedData = JsonConvert.SerializeObject(data);
            var result = await Post(requestUri, serializedData, username, password, expectedStatus, cancellationTokenSource);

            return JsonConvert.DeserializeObject<TR>(result);
        }

        public async Task<string> Post(Uri requestUri, string data, string username, string password, HttpStatusCode expectedStatus, CancellationTokenSource cancellationTokenSource)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + GetBasicCredentials(username, password));


                HttpResponseMessage response;
                if (cancellationTokenSource != null)
                    response = await client.PostAsync(requestUri, new StringContent(data, Encoding.UTF8, "application/json"), cancellationTokenSource.Token);
                else
                    response = await client.PostAsync(requestUri, new StringContent(data, Encoding.UTF8, "application/json"));

                var result = await response.Content.ReadAsStringAsync();
                Ensure(response, result, expectedStatus);

                return result;
            }
        }

        public async Task<OperationResult> PostData(Uri requestUri, string data, string username, string password, HttpStatusCode expectedStatusCode, CancellationTokenSource cancellationTokenSource)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + GetBasicCredentials(username, password));


                HttpResponseMessage response;
                if (cancellationTokenSource != null)
                    response = await client.PostAsync(requestUri, new StringContent(data, Encoding.UTF8, "application/json"), cancellationTokenSource.Token);
                else
                    response = await client.PostAsync(requestUri, new StringContent(data, Encoding.UTF8, "application/json"));

                var result = response.Content.ReadAsStringAsync().Result;
                Ensure(response, result, expectedStatusCode);

                return new OperationResult { IsValid = true };
            }
        }

        public async Task<TR> Get<TR>(Uri requestUri, string username, string password, CancellationTokenSource cancellationTokenSource)
        {
            string result = await Get(requestUri, username, password, cancellationTokenSource);

            return JsonConvert.DeserializeObject<TR>(result);
        }

        public async Task<string> Get(Uri requestUri, string username, string password, CancellationTokenSource cancellationTokenSource)
        {
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Basic " + GetBasicCredentials(username, password));

                HttpResponseMessage response;
                if (cancellationTokenSource != null)
                    response = await client.GetAsync(requestUri, cancellationTokenSource.Token);
                else
                    response = await client.GetAsync(requestUri);

                var result = await response.Content.ReadAsStringAsync();
                Ensure(response, result);

                return result;
            }
        }

        private void Ensure(HttpResponseMessage response, string result, HttpStatusCode expectedStatus = HttpStatusCode.OK)
        {
            if (response.StatusCode == expectedStatus)
                return;

            var responseMessage = "Response status code does not indicate success: " + (int)response.StatusCode + " (" + response.StatusCode + " ). ";
            throw new HttpRequestException(responseMessage + Environment.NewLine + result);
        }

        private string GetBasicCredentials(string username, string password)
        {
            string mergedCredentials = string.Format("{0}:{1}", username, password);
            byte[] byteCredentials = Encoding.UTF8.GetBytes(mergedCredentials);

            return Convert.ToBase64String(byteCredentials);
        }
    }
}
