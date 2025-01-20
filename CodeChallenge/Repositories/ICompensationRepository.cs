using CodeChallenge.Models;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation GetCompensation(string id);
        Compensation Add(Compensation compensation);
        Task SaveAsync();
    }
}
