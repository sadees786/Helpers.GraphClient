using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Cqc.Helpers.GraphClient
{
    public interface IGraphClient
    {
        Task<string> CreateUser(string emailAddress, string displayName, string firstName, string surname);
        Task<bool> UserExist(string query);
    }
}