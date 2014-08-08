using System.Threading.Tasks;
namespace Jirabox.Core.Contracts
{
    public interface ICacheDataService
    {
        Task ClearCacheData();
        void Save(string fileName, string content);
        Task<string> Get(string fileName);
        Task<bool> DoesFileExist(string fileName);
    }
}
