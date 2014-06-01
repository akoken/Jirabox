using System.Net.Http;
using System.Threading.Tasks;

namespace Jirabox.Core.Contracts
{
    public interface IHttpManager
    {
        Task<HttpResponseMessage> GetAsync(string url, bool withBasicAuthentication = false, string username = null, string password = null);

        Task<HttpResponseMessage> PostAsync(string url, string data, bool withBasicAuthentication = false, string username = null, string password = null);
    }
}
