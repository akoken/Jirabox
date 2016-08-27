using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Jirabox.Core.Contracts
{
    public interface IJsonHttpClient
    {
        Task<TR> Post<T, TR>(Uri requestUri, T data, string username, string password, HttpStatusCode expectedStatus, CancellationTokenSource cancellationTokenSource);
        Task<string> Post(Uri requestUri, string data, string username, string password, HttpStatusCode expectedStatus, CancellationTokenSource cancellationTokenSource);
        Task<TR> Get<TR>(Uri requestUri, string username, string password, CancellationTokenSource cancellationTokenSource);
        Task<string> Get(Uri requestUri, string username, string password, CancellationTokenSource cancellationTokenSource);
        Task<OperationResult> PostData(Uri requestUri, string data, string username, string password, HttpStatusCode expectedStatusCode, CancellationTokenSource cancellationTokenSource);
    }
}
