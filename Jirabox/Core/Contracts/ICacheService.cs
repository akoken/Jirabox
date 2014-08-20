
namespace Jirabox.Core.Contracts
{
    /// <summary>
    /// ICacheService
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Saves content to the cache folder
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="content"></param>
        void Save(string fileName, string content);


        /// <summary>
        /// Returns content from cache folder
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string Read(string fileName);

        /// <summary>
        /// Checks if file exists
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        bool DoesFileExist(string fileName);


        /// <summary>
        /// Clear cache folder
        /// </summary>
        void ClearCache();
    }
}
