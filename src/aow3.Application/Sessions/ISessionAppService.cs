using System.Threading.Tasks;
using Abp.Application.Services;
using aow3.Sessions.Dto;

namespace aow3.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
